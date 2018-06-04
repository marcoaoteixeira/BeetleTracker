using System;
using System.Collections.Generic;
using System.Runtime.Caching;
using SystemCacheItemPolicy = System.Runtime.Caching.CacheItemPolicy;

namespace Nameless.BeetleTracker.Caching {

    /// <summary>
    /// Default implementation of <see cref="ICache"/>.
    /// </summary>
    public sealed class InMemoryCache : ICache, IDisposable {

        #region Public Static Read-Only Fields

        /// <summary>
        /// Gets the cache name.
        /// </summary>
        public static readonly string CacheName = nameof(InMemoryCache);

        #endregion

        #region Private Fields

        private MemoryCache _cache;
        private bool _disposed;

        #endregion Private Fields

        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of <see cref="InMemoryCache"/>.
        /// </summary>
        public InMemoryCache() {
            _cache = new MemoryCache(CacheName);
        }

        #endregion Public Constructors

        #region Destructor

        /// <summary>
        /// Destructor.
        /// </summary>
        ~InMemoryCache() {
            Dispose(disposing: false);
        }

        #endregion Destructor

        #region Private Methods

        private void Dispose(bool disposing) {
            if (_disposed) { return; }
            if (disposing) {
                if (_cache != null) {
                    _cache.Dispose();
                }
            }
            _cache = null;
            _disposed = true;
        }

        private void SetEvictionCallback(SystemCacheItemPolicy policy, Action<string> callback) {
            if (callback != null) {
                policy.RemovedCallback = (args) => callback(args.CacheItem.Key);
            }
        }

        private void SetCacheDependency(SystemCacheItemPolicy policy, CacheDependency dependency) {
            if (dependency is FileCacheDependency) {
                SetFileCacheDependency(policy, dependency as FileCacheDependency);
            }

            if (dependency is TimeCacheDependency) {
                SetTimeCacheDependency(policy, dependency as TimeCacheDependency);
            }
        }

        private void SetFileCacheDependency(SystemCacheItemPolicy policy, FileCacheDependency dependency) {
            policy.ChangeMonitors.Add(new HostFileChangeMonitor(new List<string> { dependency.FilePath }));
        }

        private void SetTimeCacheDependency(SystemCacheItemPolicy policy, TimeCacheDependency dependency) {
            switch (dependency.Policy) {
                case CacheItemPolicy.AbsoluteExpiration:
                    policy.AbsoluteExpiration = DateTimeOffset.Now.Add(dependency.Time);
                    break;

                case CacheItemPolicy.SlidingExpiration:
                    policy.SlidingExpiration = dependency.Time;
                    break;
            }
        }

        #endregion Private Methods

        #region ICache Members

        /// <inheritdoc />
        public object Get(string key) => _cache.Get(key);

        /// <inheritdoc />
        public bool IsTracked(string key) => _cache.Get(key) != null;

        /// <inheritdoc />
        public void Remove(string key) {
            _cache.Remove(key);
        }

        /// <inheritdoc />
        public void Set(string key, object obj, Action<string> evictionCallback = null, CacheDependency dependency = null) {
            var policy = new SystemCacheItemPolicy();

            SetEvictionCallback(policy, evictionCallback);
            SetCacheDependency(policy, dependency);

            _cache.Set(key, obj, policy);
        }

        #endregion ICache Members

        #region IDisposable Members

        /// <inheritdoc />
        public void Dispose() {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion IDisposable Members
    }
}