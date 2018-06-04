using System;
using System.Data;
using Nameless.BeetleTracker.Data;
using Nameless.BeetleTracker.EventSourcing.Models;
using Nameless.BeetleTracker.EventSourcing.Resources;

namespace Nameless.BeetleTracker.EventSourcing.Snapshots {

    /// <summary>
    /// Default implementation of <see cref="ISnapshotStore"/>
    /// </summary>
    public sealed class SnapshotStore : ISnapshotStore {

        #region Private Read-Only Fields

        private readonly IDatabase _database;

        #endregion Private Read-Only Fields

        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of <see cref="SnapshotStore"/>
        /// </summary>
        public SnapshotStore(IDatabase database) {
            Prevent.ParameterNull(database, nameof(database));

            _database = database;
        }

        #endregion Public Constructors

        #region Private Static Methods

        private static SnapshotEntity Map(IDataReader reader) {
            return new SnapshotEntity {
                AggregateID = reader.GetGuidOrDefault(nameof(SnapshotEntity.AggregateID)),
                Version = reader.GetInt32OrDefault(nameof(SnapshotEntity.Version)),
                SnapshotType = reader.GetStringOrDefault(nameof(SnapshotEntity.SnapshotType)),
                Payload = reader.GetBlobOrDefault(nameof(SnapshotEntity.Payload))
            };
        }

        #endregion Private Static Methods

        #region ISnapshotStore Members

        /// <inheritdoc />
        public Snapshot Get(Guid id) {
            var snapshot = _database.ExecuteReaderSingle(SQL.ListSnapshots, Map, parameters: new[] {
                Parameter.CreateInputParameter(nameof(EventEntity.AggregateID), id, DbType.Guid)
            });
            return snapshot.GetSnapshotFromPayload();
        }

        /// <inheritdoc />
        public void Save(Snapshot snapshot) {
            var entity = SnapshotEntity.Create(snapshot);
            _database.ExecuteNonQuery(SQL.CreateSnapshot, parameters: new[] {
                Parameter.CreateInputParameter(nameof(SnapshotEntity.AggregateID), entity.AggregateID, DbType.Guid),
                Parameter.CreateInputParameter(nameof(SnapshotEntity.Version), entity.Version, DbType.Int32),
                Parameter.CreateInputParameter(nameof(SnapshotEntity.SnapshotType), entity.SnapshotType),
                Parameter.CreateInputParameter(nameof(SnapshotEntity.Payload), entity.Payload, DbType.Binary)
            });
        }

        #endregion ISnapshotStore Members
    }
}