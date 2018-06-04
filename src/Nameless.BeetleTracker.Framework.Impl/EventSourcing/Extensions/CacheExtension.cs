using System;
using Nameless.BeetleTracker.Caching;

namespace Nameless.BeetleTracker.EventSourcing {

    internal static class CacheExtension {

        #region Internal Static Methods

        internal static bool IsTracked(this ICache cache, Guid key) {
            if (cache == null) { return false; }

            return cache.IsTracked(key.ToString());
        }

        internal static object Get(this ICache cache, Guid key) {
            if (cache == null) { return null; }

            return cache.Get(key.ToString());
        }

        internal static void Remove(this ICache cache, Guid key) {
            if (cache == null) { return; }

            cache.Remove(key.ToString());
        }
        
        internal static void Set(this ICache cache, Guid key, object obj, Action<Guid> evictionCallback = null, CacheDependency dependency = null) {
            if (cache == null) { return; }

            Action<string> callback = null;
            if (evictionCallback != null) {
                callback = _ => evictionCallback(Guid.Parse(_));
            }
            cache.Set(key.ToString(), obj, callback, dependency);
        }

        #endregion Internal Static Methods
    }
}