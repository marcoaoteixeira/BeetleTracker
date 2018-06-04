using System;
using System.Data.Common;
using Nameless.BeetleTracker.IoC;

namespace Nameless.BeetleTracker.Data {

    /// <summary>
    /// Default implementation of <see cref="IDbProviderSelector"/>
    /// </summary>
    public class DbProviderSelector : IDbProviderSelector {

        #region Private Read-Only Fields

        private readonly IResolver _resolver;

        #endregion Private Read-Only Fields

        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of <see cref="DbProviderSelector"/>
        /// </summary>
        /// <param name="resolver">The IoC resolver instance.</param>
        public DbProviderSelector(IResolver resolver) {
            Prevent.ParameterNull(resolver, nameof(resolver));

            _resolver = resolver;
        }

        #endregion Public Constructors

        #region IDbProvider Members

        /// <inheritdoc/>
        public DbProviderFactory GetFactory(Type providerFactoryType) {
            Prevent.ParameterNull(providerFactoryType, nameof(providerFactoryType));

            var dbProviderFactory = _resolver.Resolve(providerFactoryType, name: providerFactoryType.FullName);
            return (DbProviderFactory)dbProviderFactory;
        }

        /// <inheritdoc/>
        public DbProviderFactory GetFactory(string providerFactoryName) {
            Prevent.ParameterNullOrWhiteSpace(providerFactoryName, nameof(providerFactoryName));

            var dbProviderFactory = _resolver.Resolve<DbProviderFactory>(name: providerFactoryName);
            return dbProviderFactory;
        }

        #endregion IDbProvider Members
    }
}