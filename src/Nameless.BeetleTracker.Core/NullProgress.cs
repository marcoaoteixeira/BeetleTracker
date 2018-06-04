using System;

namespace Nameless.BeetleTracker {

    /// <summary>
    /// Null Object Pattern implementation for <see cref="IProgress{T}"/>. (see: https://en.wikipedia.org/wiki/Null_Object_pattern)
    /// </summary>
    public sealed class NullProgress<T> : IProgress<T> {

        #region Private Static Read-Only Fields

        private static readonly IProgress<T> _instance = new NullProgress<T>();

        #endregion Private Static Read-Only Fields

        #region Public Static Properties

        /// <summary>
        /// Gets the unique instance of <see cref="NullProgress{T}"/>.
        /// </summary>
        public static IProgress<T> Instance => _instance;

        #endregion Public Static Properties

        #region Static Constructors

        // Explicit static constructor to tell the C# compiler
        // not to mark type as beforefieldinit
        static NullProgress() {
        }

        #endregion Static Constructors

        #region Private Constructors

        // Prevents the class from being constructed.
        private NullProgress() {
        }

        #endregion Private Constructors

        #region IProgress<T> Members

        /// <inheritdoc />
        public void Report(T value) {
        }

        #endregion IProgress<T> Members
    }
}