using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using Nameless.BeetleTracker.Caching;
using Nameless.BeetleTracker.EventSourcing.Events;

namespace Nameless.BeetleTracker.EventSourcing.Domains {

    /// <summary>
    /// In-memory implementation of <see cref="IRepository"/>.
    /// </summary>
    public sealed class InMemoryRepository : IRepository {

        #region Private Static Read-Only Fields

        private static readonly ConcurrentDictionary<Guid, SemaphoreSlim> Locks = new ConcurrentDictionary<Guid, SemaphoreSlim>();

        #endregion Private Static Read-Only Fields

        #region Private Read-Only Fields

        private readonly ICache _cache;
        private readonly IEventStore _eventStore;
        private readonly IRepository _repository;

        #endregion Private Read-Only Fields

        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of <see cref="InMemoryRepository"/>.
        /// </summary>
        /// <param name="cache"></param>
        /// <param name="eventStore"></param>
        /// <param name="repository"></param>
        public InMemoryRepository(ICache cache, IEventStore eventStore, IRepository repository) {
            Prevent.ParameterNull(cache, nameof(cache));
            Prevent.ParameterNull(eventStore, nameof(eventStore));
            Prevent.ParameterNull(repository, nameof(repository));

            _cache = cache;
            _eventStore = eventStore;
            _repository = repository;
        }

        #endregion Public Constructors

        #region Private Static Methods

        private static SemaphoreSlim CreateLock(Guid key) => new SemaphoreSlim(1, 1);

        #endregion Private Static Methods

        #region IRepository Members

        /// <inheritdoc />
        public TAggregate Get<TAggregate>(Guid aggregateID) where TAggregate : AggregateRoot {
            var @lock = Locks.GetOrAdd(aggregateID, CreateLock);

            @lock.Wait();
            try {
                TAggregate aggregate;
                if (_cache.IsTracked(aggregateID)) {
                    aggregate = (TAggregate)_cache.Get(aggregateID);
                    var events = _eventStore.Get(aggregateID, aggregate.Version);
                    if (events.Any() && events.First().Version != aggregate.Version + 1) {
                        _cache.Remove(aggregateID);
                    } else {
                        aggregate.LoadFromHistory(events);
                        return aggregate;
                    }
                }

                aggregate = _repository.Get<TAggregate>(aggregateID);
                SemaphoreSlim dummy;
                _cache.Set(aggregateID, aggregate, key => Locks.TryRemove(key, out dummy));
                return aggregate;
            }
            catch (Exception) { _cache.Remove(aggregateID); throw; }
            finally { @lock.Release(); }
        }

        /// <inheritdoc />
        public void Save<TAggregate>(TAggregate aggregate, int? expectedVersion = default(int?)) where TAggregate : AggregateRoot {
            var @lock = Locks.GetOrAdd(aggregate.ID, CreateLock);
            @lock.Wait();
            try {
                if (aggregate.ID != Guid.Empty && !_cache.IsTracked(aggregate.ID.ToString())) {
                    _cache.Set(aggregate.ID, aggregate);
                }
                _repository.Save(aggregate, expectedVersion);
            }
            catch (Exception) { _cache.Remove(aggregate.ID); throw; }
            finally { @lock.Release(); }
        }

        #endregion IRepository Members
    }
}