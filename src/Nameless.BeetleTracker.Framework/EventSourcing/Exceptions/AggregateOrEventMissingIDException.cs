using System;

namespace Nameless.BeetleTracker.EventSourcing {

    /// <summary>
    /// Exception for aggregate and event without ID.
    /// </summary>
    public class AggregateOrEventMissingIDException : Exception {

        #region Public Properties

        /// <summary>
        /// Gets the aggregate type.
        /// </summary>
        public Type AggregateType { get; }

        /// <summary>
        /// Gets the event type.
        /// </summary>
        public Type EventType { get; }

        #endregion Public Properties

        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of <see cref="AggregateOrEventMissingIDException"/>
        /// </summary>
        public AggregateOrEventMissingIDException(Type aggregateType, Type eventType) {
            AggregateType = aggregateType;
            EventType = eventType;
        }

        /// <summary>
        /// Initializes a new instance of <see cref="AggregateOrEventMissingIDException"/>
        /// </summary>
        public AggregateOrEventMissingIDException() { }

        /// <summary>
        /// Initializes a new instance of <see cref="AggregateOrEventMissingIDException"/>
        /// </summary>
        /// <param name="message">The exception message</param>
        public AggregateOrEventMissingIDException(string message) : base(message) { }

        /// <summary>
        /// Initializes a new instance of <see cref="AggregateOrEventMissingIDException"/>
        /// </summary>
        /// <param name="message">The exception message.</param>
        /// <param name="innerException">The inner exception.</param>
        public AggregateOrEventMissingIDException(string message, Exception innerException) : base(message, innerException) { }

        #endregion Public Constructors
    }
}