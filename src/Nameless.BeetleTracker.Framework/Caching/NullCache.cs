using System;

namespace Nameless.BeetleTracker.Caching {

    /// <summary>
    /// Null Object Pattern implementation for ICache. (see: https://en.wikipedia.org/wiki/Null_Object_pattern)
    /// </summary>
    public sealed class NullCache : ICache {

        #region Private Static Read-Only Fields

        private static readonly ICache _instance = new NullCache();

        #endregion Private Static Read-Only Fields

        #region Public Static Properties

        /// <summary>
        /// Gets the unique instance of NullCache.
        /// </summary>
        public static ICache Instance => _instance;

        #endregion Public Static Properties

        #region Static Constructors

        // Explicit static constructor to tell the C# compiler
        // not to mark type as beforefieldinit
        static NullCache() {
        }

        #endregion Static Constructors

        #region Private Constructors

        // Prevents the class from being constructed.
        private NullCache() {
        }

        #endregion Private Constructors

        #region ICache Members

        /// <inheritdoc />
        public bool IsTracked(string key) => false;

        /// <inheritdoc />
        public void Set(string key, object obj, Action<string> evictionCallback = null, CacheDependency dependency = null) {
        }

        /// <inheritdoc />
        public object Get(string key) => null;

        /// <inheritdoc />
        public void Remove(string key) {
        }

        #endregion ICache Members
    }
}