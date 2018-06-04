using System;
using Nameless.BeetleTracker.EventSourcing.Events;

namespace Nameless.BeetleTracker.EventSourcing.Domains {

    /// <summary>
    /// Default implementation of <see cref="IRepository"/>
    /// </summary>
    public sealed class Repository : IRepository {

        #region Private Read-Only Fields

        private readonly IAggregateFactory _aggregateFactory;
        private readonly IEventStore _eventStore;

        #endregion Private Read-Only Fields

        #region Public Constructors
        
        /// <summary>
        /// Initializes a new instance of <see cref="Repository"/>
        /// </summary>
        /// <param name="aggregateFactory">The aggregate factory instance.</param>
        /// <param name="eventStore">The event store instance.</param>
        public Repository(IAggregateFactory aggregateFactory, IEventStore eventStore) {
            Prevent.ParameterNull(aggregateFactory, nameof(aggregateFactory));
            Prevent.ParameterNull(eventStore, nameof(eventStore));

            _aggregateFactory = aggregateFactory;
            _eventStore = eventStore;
        }

        #endregion Public Constructors

        #region IRepository Members

        /// <inheritdoc />
        public TAggregate Get<TAggregate>(Guid aggregateID) where TAggregate : AggregateRoot {
            var evts = _eventStore.Get(aggregateID, -1);

            if (evts.IsNullOrEmpty()) {
                throw new AggregateNotFoundException(typeof(TAggregate), aggregateID);
            }

            var aggregate = _aggregateFactory.Create<TAggregate>();
            aggregate.LoadFromHistory(evts);
            return aggregate;
        }
        /// <inheritdoc />
        public void Save<TAggregate>(TAggregate aggregate, int? expectedVersion = default(int?)) where TAggregate : AggregateRoot {
            if (expectedVersion.HasValue && !_eventStore.Get(aggregate.ID, expectedVersion.Value).IsNullOrEmpty()) {
                throw new ConcurrencyException(aggregate.ID);
            }

            var changes = aggregate.FlushUncommitedChanges();

            _eventStore.Save(changes);
        }

        #endregion IRepository Members
    }
}