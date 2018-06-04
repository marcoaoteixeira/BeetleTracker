using System;

namespace Nameless.BeetleTracker.Caching {

    /// <summary>
    /// Enumerates the various cache item policies.
    /// </summary>
    public enum CacheItemPolicy : int {

        /// <summary>
        /// Indicates whether a cache entry should be evicted after a specified duration.
        /// </summary>
        AbsoluteExpiration,

        /// <summary>
        /// Indicates whether a cache entry should be evicted if it has not been accessed in a given
        /// span of time.
        /// </summary>
        SlidingExpiration
    }

    /// <summary>
    /// Defines a cache dependency based on time.
    /// </summary>
    public sealed class TimeCacheDependency : CacheDependency {

        #region Public Properties

        /// <summary>
        /// Gets the time to expire the cache.
        /// </summary>
        public TimeSpan Time { get; }

        /// <summary>
        /// Gets the cache item policy.
        /// </summary>
        public CacheItemPolicy Policy { get; }

        #endregion Public Properties

        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of <see cref="TimeCacheDependency"/>
        /// </summary>
        /// <param name="time">The time span.</param>
        /// <param name="policy">The policy.</param>
        public TimeCacheDependency(TimeSpan time, CacheItemPolicy policy = CacheItemPolicy.AbsoluteExpiration) {
            Time = time;
            Policy = policy;
        }

        #endregion Public Constructors

        #region Public Static Methods

        /// <summary>
        /// Creates a new instance of <see cref="TimeCacheDependency"/>
        /// </summary>
        /// <param name="time">The time span.</param>
        /// <param name="policy">The policy.</param>
        /// <returns>An instance of <see cref="TimeCacheDependency"/></returns>
        public static CacheDependency Create(TimeSpan time, CacheItemPolicy policy = CacheItemPolicy.AbsoluteExpiration) {
            return new TimeCacheDependency(time, policy);
        }

        #endregion Public Static Methods
    }
}