using System;

namespace Nameless.BeetleTracker.Caching {

    /// <summary>
    /// Defines methods/properties/events to implement a cache tracker.
    /// </summary>
    public interface ICache {

        #region Methods

        /// <summary>
        /// Checks if the entry is tracked.
        /// </summary>
        /// <param name="key">The cache entry key.</param>
        /// <returns><c>true</c> if is tracked; otherwise, <c>false</c>.</returns>
        bool IsTracked(string key);

        /// <summary>
        /// Sets an entry into the cache.
        /// </summary>
        /// <param name="key">The entry key.</param>
        /// <param name="obj">The object.</param>
        /// <param name="evictionCallback">A callback that will be called when an entry is evicted from the cache</param>
        /// <param name="dependency">The cache dependency.</param>
        void Set(string key, object obj, Action<string> evictionCallback = null, CacheDependency dependency = null);

        /// <summary>
        /// Retrieves an entry from the cache.
        /// </summary>
        /// <param name="key">The entry key.</param>
        /// <returns>The object.</returns>
        object Get(string key);

        /// <summary>
        /// Removes an entry from the cache.
        /// </summary>
        /// <param name="key">The entry key.</param>
        void Remove(string key);

        #endregion Methods
    }
}