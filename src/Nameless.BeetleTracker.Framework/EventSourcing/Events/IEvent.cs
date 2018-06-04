using System;
using Nameless.BeetleTracker.EventSourcing.Messaging;

namespace Nameless.BeetleTracker.EventSourcing.Events {

    /// <summary>
    /// Interface for an event.
    /// </summary>
    public interface IEvent : IMessage {

        #region Properties

        /// <summary>
        /// Gets or sets the event ID.
        /// </summary>
        Guid ID { get; set; }

        /// <summary>
        /// Gets or sets the event version.
        /// </summary>
        int Version { get; set; }

        /// <summary>
        /// Gets or sets the event time stamp.
        /// </summary>
        DateTimeOffset TimeStamp { get; set; }

        /// <summary>
        /// Gets or sets the event owner.
        /// </summary>
        Guid Owner { get; set; }

        #endregion Properties
    }
}