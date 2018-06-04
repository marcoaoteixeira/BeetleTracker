using System;
using Nameless.BeetleTracker.EventSourcing.Snapshots;

namespace Nameless.BeetleTracker.EventSourcing.Models {

    /// <summary>
    /// Snapshot entry entity.
    /// </summary>
    public class SnapshotEntity {

        #region Public Properties

        /// <summary>
        /// Gets or sets the aggregate ID.
        /// </summary>
        public Guid AggregateID { get; set; }

        /// <summary>
        /// Gets or sets the snapshot version.
        /// </summary>
        public int Version { get; set; }

        /// <summary>
        /// Gets or sets the snapshot type.
        /// </summary>
        public string SnapshotType { get; set; }

        /// <summary>
        /// Gets or sets the snapshot payload.
        /// </summary>
        public byte[] Payload { get; set; }

        /// <summary>
        /// Gets or sets the event owner.
        /// </summary>
        public Guid Owner { get; set; }

        #endregion Public Virtual Properties

        #region Public Static Methods

        /// <summary>
        /// Creates an instance of <see cref="SnapshotEntity"/> from a <see cref="Snapshot"/>.
        /// </summary>
        /// <param name="snapshot">The <see cref="Snapshot"/> instance.</param>
        /// <returns>An instance of <see cref="SnapshotEntity"/> representing the <see cref="Snapshot"/>.</returns>
        public static SnapshotEntity Create(Snapshot snapshot) => new SnapshotEntity {
            AggregateID = snapshot.ID,
            Version = snapshot.Version,
            SnapshotType = snapshot.GetType().FullName,
            Payload = SerializerHelper.Serialize(snapshot),
            Owner = snapshot.Owner
        };

        #endregion Public Static Methods

        #region Public Methods

        /// <summary>
        /// Retrieves an instance of <see cref="Snapshot"/> from the <see cref="Payload"/>.
        /// </summary>
        /// <typeparam name="TSnapshot">Type of the snapshot.</typeparam>
        /// <returns>An instance of <see cref="Snapshot"/>.</returns>
        public TSnapshot GetSnapshotFromPayload<TSnapshot>() where TSnapshot : Snapshot => SerializerHelper.Deserialize<TSnapshot>(Payload);

        /// <summary>
        /// Retrieves an instance of <see cref="Snapshot"/> from the <see cref="Payload"/>.
        /// </summary>
        /// <returns>An instance of <see cref="Snapshot"/>.</returns>
        public Snapshot GetSnapshotFromPayload() => (Snapshot)SerializerHelper.Deserialize(Payload, SnapshotType);

        #endregion Public Virtual Methods
    }
}