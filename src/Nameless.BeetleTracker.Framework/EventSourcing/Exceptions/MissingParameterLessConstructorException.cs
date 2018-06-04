using System;

namespace Nameless.BeetleTracker.EventSourcing {

    /// <summary>
    /// Exception for missing parameterless constructor.
    /// </summary>
    public class MissingParameterlessConstructorException : Exception {

        #region Public Properties

        /// <summary>
        /// Gets the type.
        /// </summary>
        public Type Type { get; }

        #endregion Public Properties

        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of <see cref="MissingParameterlessConstructorException"/>
        /// </summary>
        /// <param name="type">The type being constructed.</param>
        public MissingParameterlessConstructorException(Type type) {
            Type = type;
        }

        /// <summary>
        /// Initializes a new instance of <see cref="MissingParameterlessConstructorException"/>
        /// </summary>
        public MissingParameterlessConstructorException() { }

        /// <summary>
        /// Initializes a new instance of <see cref="MissingParameterlessConstructorException"/>
        /// </summary>
        /// <param name="message">The exception message</param>
        public MissingParameterlessConstructorException(string message) : base(message) { }

        /// <summary>
        /// Initializes a new instance of <see cref="MissingParameterlessConstructorException"/>
        /// </summary>
        /// <param name="message">The exception message.</param>
        /// <param name="innerException">The inner exception.</param>
        public MissingParameterlessConstructorException(string message, Exception innerException) : base(message, innerException) { }

        #endregion Public Constructors
    }
}