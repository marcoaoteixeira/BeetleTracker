using System;
using Nameless.BeetleTracker.EventSourcing.Domains;

namespace Nameless.BeetleTracker.EventSourcing.Snapshots {

    /// <summary>
    /// Abstract class to define a snapshot aggregate root.
    /// </summary>
    /// <typeparam name="TSnapshot"></typeparam>
    public abstract class SnapshotAggregateRoot<TSnapshot> : AggregateRoot where TSnapshot : Snapshot {

        #region Public Methods

        /// <summary>
        /// Gets the snapshot of the current aggregate root.
        /// </summary>
        /// <returns>The snapshot.</returns>
        public TSnapshot GetSnapshot() {
            return CreateSnapshot(ID);
        }

        /// <summary>
        /// Restores the aggregate root from a snapshot point.
        /// </summary>
        /// <param name="snapshot">The snapshot.</param>
        public void Restore(TSnapshot snapshot) {
            ID = snapshot.ID;
            Version = snapshot.Version;

            RestoreFromSnapshot(snapshot);
        }

        #endregion Public Methods

        #region Protected Abstract Methods

        /// <summary>
        /// Creates a snapshot from the aggregate root.
        /// </summary>
        /// <param name="aggregateID">The aggregate ID.</param>
        /// <returns>The snapshot.</returns>
        protected abstract TSnapshot CreateSnapshot(Guid aggregateID);

        /// <summary>
        /// Restores the aggregate root from a snapshot point.
        /// </summary>
        /// <param name="snapshot">The snapshot</param>
        protected abstract void RestoreFromSnapshot(TSnapshot snapshot);

        #endregion Protected Abstract Methods
    }
}