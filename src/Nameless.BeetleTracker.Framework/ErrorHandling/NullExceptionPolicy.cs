using System;

namespace Nameless.BeetleTracker.ErrorHandling {

    /// <summary>
    /// Null Object Pattern implementation for IExceptionPolicy. (see: https://en.wikipedia.org/wiki/Null_Object_pattern)
    /// </summary>
    public sealed class NullExceptionPolicy : IExceptionPolicy {

        #region Private Static Read-Only Fields

        private static readonly IExceptionPolicy _instance = new NullExceptionPolicy();

        #endregion Private Static Read-Only Fields

        #region Public Static Properties

        /// <summary>
        /// Gets the unique instance of NullExceptionPolicy.
        /// </summary>
        public static IExceptionPolicy Instance => _instance;

        #endregion Public Static Properties

        #region Static Constructors

        // Explicit static constructor to tell the C# compiler
        // not to mark type as beforefieldinit
        static NullExceptionPolicy() {
        }

        #endregion Static Constructors

        #region Private Constructors

        // Prevents the class from being constructed.
        private NullExceptionPolicy() {
        }

        #endregion Private Constructors

        #region IExceptionPolicy Members

        /// <inheritdoc />
        public bool Handle(object sender, Exception exception) => true;

        #endregion IExceptionPolicy Members
    }
}