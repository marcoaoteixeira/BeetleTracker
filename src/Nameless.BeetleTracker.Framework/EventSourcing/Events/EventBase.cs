using System;

namespace Nameless.BeetleTracker.EventSourcing.Events {

    /// <summary>
    /// Abstract implementation of <see cref="IEvent"/>
    /// </summary>
    public abstract class EventBase : IEvent {

        #region IEvent Members

        /// <inheritdoc />
        public Guid ID { get; set; }

        /// <inheritdoc />
        public int Version { get; set; }

        /// <inheritdoc />
        public DateTimeOffset TimeStamp { get; set; }

        /// <inheritdoc />
        public Guid Owner { get; set; }

        #endregion IEvent Members

        #region Public Override Methods

        /// <inheritdoc />
        public override string ToString() {
            return GetType().Name;
        }

        #endregion Public Override Methods
    }
}