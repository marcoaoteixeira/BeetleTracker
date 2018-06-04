using System;

namespace Nameless.BeetleTracker.EventSourcing {

    /// <summary>
    /// Exception for aggregate concurrency cases.
    /// </summary>
    public class AggregateNotFoundException : Exception {

        #region Public Properties

        /// <summary>
        /// Gets the aggregate type.
        /// </summary>
        public Type AggregateType { get; }

        /// <summary>
        /// Gets the aggregate ID.
        /// </summary>
        public Guid AggregateID { get; }

        #endregion Public Properties

        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of <see cref="AggregateNotFoundException"/>
        /// </summary>
        /// <param name="aggregateType">Type of the aggregate.</param>
        /// <param name="aggregateID">The aggregate ID.</param>
        public AggregateNotFoundException(Type aggregateType, Guid aggregateID)
            : base(string.Format(Properties.Resources.AggregateNotFoundMessage, aggregateID, aggregateType)) {
            AggregateType = aggregateType;
            AggregateID = aggregateID;
        }

        /// <summary>
        /// Initializes a new instance of <see cref="AggregateNotFoundException"/>
        /// </summary>
        public AggregateNotFoundException() { }

        /// <summary>
        /// Initializes a new instance of <see cref="AggregateNotFoundException"/>
        /// </summary>
        /// <param name="message">The exception message</param>
        public AggregateNotFoundException(string message) : base(message) { }

        /// <summary>
        /// Initializes a new instance of <see cref="AggregateNotFoundException"/>
        /// </summary>
        /// <param name="message">The exception message.</param>
        /// <param name="innerException">The inner exception.</param>
        public AggregateNotFoundException(string message, Exception innerException) : base(message, innerException) { }

        #endregion Public Constructors
    }
}