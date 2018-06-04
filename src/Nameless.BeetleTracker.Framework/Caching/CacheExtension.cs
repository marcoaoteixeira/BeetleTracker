using System;

namespace Nameless.BeetleTracker.Caching {

    /// <summary>
    /// Extension methods for <see cref="ICache"/>
    /// </summary>
    public static class CacheExtension {

        #region Public Static Methods

        /// <summary>
        /// Retrieves an entry from the cache.
        /// </summary>
        /// <typeparam name="T">Type of the entry.</typeparam>
        /// <param name="source">The <see cref="ICache"/> implementation.</param>
        /// <param name="key">The entry key.</param>
        /// <returns>The entry.</returns>
        public static T Get<T>(this ICache source, string key) {
            if (source == null) { return default(T); }

            return (T)source.Get(key);
        }

        #endregion Public Static Methods
    }
}