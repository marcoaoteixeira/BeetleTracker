using System;
using Nameless.BeetleTracker.EventSourcing.Events;

namespace Nameless.BeetleTracker.EventSourcing.Models {

    /// <summary>
    /// Event entry entity.
    /// </summary>
    public class EventEntity {

        #region Public Virtual Properties

        /// <summary>
        /// Gets or sets the aggregate Id.
        /// </summary>
        public Guid AggregateID { get; set; }

        /// <summary>
        /// Gets or sets the event version.
        /// </summary>
        public int Version { get; set; }

        /// <summary>
        /// Gets or sets the event time stamp.
        /// </summary>
        public DateTimeOffset TimeStamp { get; set; }

        /// <summary>
        /// Gets or sets the event type.
        /// </summary>
        public string EventType { get; set; }

        /// <summary>
        /// Gets or sets the event payload.
        /// </summary>
        public byte[] Payload { get; set; }

        /// <summary>
        /// Gets or sets the event owner.
        /// </summary>
        public Guid Owner { get; set; }

        #endregion Public Virtual Properties

        #region Public Static Methods

        /// <summary>
        /// Creates a new event entry based on an <see cref="IEvent"/>.
        /// </summary>
        /// <param name="evt">The event.</param>
        /// <returns>An instance of <see cref="EventEntity"/> representing the <see cref="IEvent"/>.</returns>
        public static EventEntity Create(IEvent evt) => new EventEntity {
            AggregateID = evt.ID,
            Version = evt.Version,
            TimeStamp = evt.TimeStamp,
            EventType = evt.GetType().FullName,
            Payload = SerializerHelper.Serialize(evt),
            Owner = evt.Owner
        };

        #endregion Public Static Methods

        #region Public Methods

        /// <summary>
        /// Retrieves an instance of <see cref="IEvent"/> from the current <see cref="Payload"/>.
        /// </summary>
        /// <typeparam name="TEvent">Type of the event.</typeparam>
        /// <returns>An instance of <see cref="IEvent"/>.</returns>
        public TEvent GetEventFromPayload<TEvent>() where TEvent : IEvent => SerializerHelper.Deserialize<TEvent>(Payload);

        /// <summary>
        /// Retrieves an instance of <see cref="IEvent"/> from the current <see cref="Payload"/>.
        /// </summary>
        /// <returns>An instance of <see cref="IEvent"/>.</returns>
        public IEvent GetEventFromPayload() => (IEvent)SerializerHelper.Deserialize(Payload, EventType);

        #endregion Public Methods
    }
}