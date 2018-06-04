using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Nameless.BeetleTracker.Data;
using Nameless.BeetleTracker.EventSourcing.Models;
using Nameless.BeetleTracker.EventSourcing.Resources;

namespace Nameless.BeetleTracker.EventSourcing.Events {

    /// <summary>
    /// Default implementation of <see cref="IEventStore"/>
    /// </summary>
    public class EventStore : IEventStore {

        #region Private Read-Only Fields

        private readonly IDatabase _database;
        private readonly IEventPublisher _publisher;

        #endregion Private Read-Only Fields

        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of <see cref="EventStore"/>
        /// </summary>
        public EventStore(IDatabase database, IEventPublisher publisher) {
            Prevent.ParameterNull(database, nameof(database));
            Prevent.ParameterNull(publisher, nameof(publisher));

            _database = database;
            _publisher = publisher;
        }

        #endregion Public Constructors

        #region Private Static Methods

        private static EventEntity Map(IDataReader reader) {
            return new EventEntity {
                AggregateID = reader.GetGuidOrDefault(nameof(EventEntity.AggregateID)),
                EventType = reader.GetStringOrDefault(nameof(EventEntity.EventType)),
                Payload = reader.GetBlobOrDefault(nameof(EventEntity.Payload)),
                TimeStamp = reader.GetDateTimeOffsetOrDefault(nameof(EventEntity.TimeStamp), DateTimeOffset.MinValue),
                Version = reader.GetInt32OrDefault(nameof(EventEntity.Version))
            };
        }

        #endregion Private Static Methods

        #region IEventStore Members

        /// <inheritdoc/>
        public IEnumerable<IEvent> Get(Guid aggregateID, int fromVersion) {
            var events = _database.ExecuteReader(SQL.ListEvents, Map, parameters: new[] {
                Parameter.CreateInputParameter(nameof(EventEntity.AggregateID), aggregateID, DbType.Guid),
                Parameter.CreateInputParameter(nameof(EventEntity.Version), fromVersion, DbType.Int32)
            });
            return events.Select(_ => _.GetEventFromPayload());
        }

        /// <inheritdoc/>
        public void Save(params IEvent[] evts) {
            foreach (var evt in evts) {
                var entity = EventEntity.Create(evt);
                _database.ExecuteNonQuery(SQL.CreateEvent, parameters: new[] {
                    Parameter.CreateInputParameter(nameof(EventEntity.AggregateID), entity.AggregateID, DbType.Guid),
                    Parameter.CreateInputParameter(nameof(EventEntity.Version), entity.Version, DbType.Int32),
                    Parameter.CreateInputParameter(nameof(EventEntity.Payload), entity.Payload, DbType.Binary),
                    Parameter.CreateInputParameter(nameof(EventEntity.TimeStamp), entity.TimeStamp, DbType.DateTimeOffset),
                    Parameter.CreateInputParameter(nameof(EventEntity.EventType), entity.EventType)
                });
                _publisher.Publish(evt);
            }
        }

        #endregion IEventStore Members
    }
}