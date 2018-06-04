using System;

namespace Nameless.BeetleTracker.EventSourcing {

    /// <summary>
    /// Exception for aggregate concurrency cases.
    /// </summary>
    public class ConcurrencyException : Exception {

        #region Public Properties

        /// <summary>
        /// Gets the aggregate ID.
        /// </summary>
        public Guid AggregateID { get; }

        #endregion Public Properties

        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of <see cref="ConcurrencyException"/>
        /// </summary>
        /// <param name="aggregateID">The aggregate ID.</param>
        public ConcurrencyException(Guid aggregateID)
            : base(string.Format(Properties.Resources.ADifferentVersionThanExceptedWasFoundInAggregate, aggregateID)) {
            AggregateID = aggregateID;
        }

        /// <summary>
        /// Initializes a new instance of <see cref="ConcurrencyException"/>
        /// </summary>
        public ConcurrencyException() { }

        /// <summary>
        /// Initializes a new instance of <see cref="ConcurrencyException"/>
        /// </summary>
        /// <param name="message">The exception message</param>
        public ConcurrencyException(string message) : base(message) { }

        /// <summary>
        /// Initializes a new instance of <see cref="ConcurrencyException"/>
        /// </summary>
        /// <param name="message">The exception message.</param>
        /// <param name="innerException">The inner exception.</param>
        public ConcurrencyException(string message, Exception innerException) : base(message, innerException) { }

        #endregion Public Constructors
    }
}