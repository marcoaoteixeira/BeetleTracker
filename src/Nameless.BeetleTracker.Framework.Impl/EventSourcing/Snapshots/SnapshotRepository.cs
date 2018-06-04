using System;
using System.Linq;
using Nameless.BeetleTracker.EventSourcing.Domains;
using Nameless.BeetleTracker.EventSourcing.Events;

namespace Nameless.BeetleTracker.EventSourcing.Snapshots {

    /// <summary>
    /// The default snapshot repository implementation.
    /// </summary>
    public sealed class SnapshotRepository : IRepository {

        #region Private Read-Only Fields

        private readonly IAggregateFactory _aggregateFactory;
        private readonly IEventStore _eventStore;
        private readonly IRepository _repository;
        private readonly ISnapshotStore _snapshotStore;
        private readonly ISnapshotStrategy _snapshotStrategy;

        #endregion Private Read-Only Fields

        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of <see cref="SnapshotRepository"/>
        /// </summary>
        /// <param name="aggregateFactory">The aggregate factory implementation.</param>
        /// <param name="eventStore">The event store implementation.</param>
        /// <param name="repository">The repository implementation.</param>
        /// <param name="snapshotStore">The snapshot store implementation.</param>
        /// <param name="snapshotStrategy">The snapshot strategy implementation.</param>
        public SnapshotRepository(IAggregateFactory aggregateFactory, IEventStore eventStore, IRepository repository, ISnapshotStore snapshotStore, ISnapshotStrategy snapshotStrategy) {
            Prevent.ParameterNull(aggregateFactory, nameof(aggregateFactory));
            Prevent.ParameterNull(eventStore, nameof(eventStore));
            Prevent.ParameterNull(repository, nameof(repository));
            Prevent.ParameterNull(snapshotStore, nameof(snapshotStore));
            Prevent.ParameterNull(snapshotStrategy, nameof(snapshotStrategy));

            _aggregateFactory = aggregateFactory;
            _eventStore = eventStore;
            _repository = repository;
            _snapshotStore = snapshotStore;
            _snapshotStrategy = snapshotStrategy;
        }

        #endregion Public Constructors

        #region Private Methods

        private int TryRestoreAggregateFromSnapshot<TAggregate>(Guid aggregateID, TAggregate aggregate) where TAggregate : AggregateRoot {
            var version = -1;
            if (!_snapshotStrategy.IsSnapshotable(typeof(TAggregate))) { return version; }
            var snapshot = _snapshotStore.Get(aggregateID);
            if (snapshot == null) { return version; }
            aggregate.AsDynamic().Restore(snapshot);
            version = snapshot.Version;
            return version;
        }

        private void TryMakeSnapshot(AggregateRoot aggregate) {
            if (!_snapshotStrategy.ShouldMakeSnapshot(aggregate)) { return; }
            var snapshot = aggregate.AsDynamic().GetSnapshot();
            snapshot.Version = aggregate.Version + aggregate.GetUncommittedChanges().Length;
            _snapshotStore.Save(snapshot);
        }

        #endregion Private Methods

        #region IRepository Members

        /// <inheritdoc />
        public TAggregate Get<TAggregate>(Guid aggregateID) where TAggregate : AggregateRoot {
            var aggregate = _aggregateFactory.Create<TAggregate>();
            var snapshotVersion = TryRestoreAggregateFromSnapshot(aggregateID, aggregate);
            if (snapshotVersion == -1) {
                return _repository.Get<TAggregate>(aggregateID);
            }
            var events = _eventStore.Get(aggregateID, snapshotVersion).Where(evt => evt.Version > snapshotVersion);
            aggregate.LoadFromHistory(events);
            return aggregate;
        }
        
        /// <inheritdoc />
        public void Save<TAggregate>(TAggregate aggregate, int? expectedVersion = default(int?)) where TAggregate : AggregateRoot {
            TryMakeSnapshot(aggregate);
            _repository.Save(aggregate, expectedVersion);
        }

        #endregion IRepository Members
    }
}