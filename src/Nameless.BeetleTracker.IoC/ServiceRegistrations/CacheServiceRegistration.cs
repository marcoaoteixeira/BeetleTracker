using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac;
using Nameless.BeetleTracker.Caching;

namespace Nameless.BeetleTracker.IoC.ServiceRegistrations {

    /// <summary>
    /// Autofac module implementation for Nameless.BeetleTracker.Caching namespace.
    /// </summary>
    public class CacheServiceRegistration : ServiceRegistrationBase {

        #region Public Properties

        /// <summary>
        /// Gets or sets the <see cref="ICache"/> implementation.
        /// </summary>
        /// <remarks>Default is <see cref="InMemoryCache"/>.</remarks>
        public Type CacheImplementation { get; set; } = typeof(InMemoryCache);

        /// <summary>
        /// Gets or sets the <see cref="ICache"/><see cref="LifetimeScopeType"/>.
        /// </summary>
        /// <remarks>Default is <see cref="LifetimeScopeType.PerScope"/>.</remarks>
        public LifetimeScopeType CacheLifetimeScope { get; set; } = LifetimeScopeType.Singleton;

        #endregion Public Properties

        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of <see cref="CacheServiceRegistration"/>.
        /// </summary>
        public CacheServiceRegistration()
            : base(null) { }

        /// <summary>
        /// Initializes a new instance of <see cref="CacheServiceRegistration"/>.
        /// </summary>
        /// <param name="supportAssemblies">The support assemblies.</param>
        public CacheServiceRegistration(IEnumerable<Assembly> supportAssemblies)
            : base(supportAssemblies) { }

        #endregion Public Constructors

        #region Public Override Methods

        /// <inheritdoc/>
        public override void Register() {
            var cacheImplementation = GetCacheImplementation();
            if (cacheImplementation != null) {
                Builder
                    .RegisterType(cacheImplementation)
                    .As<ICache>()
                    .SetLifetimeScope(CacheLifetimeScope);
            } else {
                Builder
                    .RegisterInstance(NullCache.Instance)
                    .As<ICache>()
                    .SetLifetimeScope(CacheLifetimeScope);
            }
        }

        #endregion Public Override Methods

        #region Private Methods

        private Type GetCacheImplementation() {
            return CacheImplementation == null
                ? GetImplementationsFromSupportAssemblies(typeof(ICache)).SingleOrDefault()
                : CacheImplementation;
        }

        #endregion Private Methods
    }
}