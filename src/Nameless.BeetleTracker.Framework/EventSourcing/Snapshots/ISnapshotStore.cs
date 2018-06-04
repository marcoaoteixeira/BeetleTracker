using System;

namespace Nameless.BeetleTracker.EventSourcing.Snapshots {

    /// <summary>
    /// Defines methods for the snapshot store.
    /// </summary>
    public interface ISnapshotStore {

        #region Methods

        /// <summary>
        /// Retrieves a snapshot by its ID.
        /// </summary>
        /// <param name="id">The snapshot ID.</param>
        /// <returns>The snapshot instance.</returns>
        Snapshot Get(Guid id);

        /// <summary>
        /// Saves a snapshot.
        /// </summary>
        /// <param name="snapshot">The snapshot.</param>
        void Save(Snapshot snapshot);

        #endregion Methods
    }
}