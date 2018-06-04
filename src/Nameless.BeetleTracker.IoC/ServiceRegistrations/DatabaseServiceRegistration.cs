using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using Autofac;
using Autofac.Extras.DynamicProxy;
using Nameless.BeetleTracker.Data;

namespace Nameless.BeetleTracker.IoC.ServiceRegistrations {

    /// <summary>
    /// Database service registration.
    /// </summary>
    public class DatabaseServiceRegistration : ServiceRegistrationBase {

        #region Public Properties

        /// <summary>
        /// Gets or sets the <see cref="IDatabase"/> implementation.
        /// </summary>
        /// <remarks>Default is <see cref="Database"/>.</remarks>
        public Type DatabaseImplementation { get; set; } = typeof(Database);

        /// <summary>
        /// Gets or sets the <see cref="IDatabase"/><see cref="LifetimeScopeType"/>.
        /// </summary>
        /// <remarks>Default is <see cref="LifetimeScopeType.Singleton"/>.</remarks>
        public LifetimeScopeType DatabaseLifetimeScope { get; set; } = LifetimeScopeType.Singleton;

        /// <summary>
        /// Gets or sets the <see cref="IDbProviderSelector"/> implementation.
        /// </summary>
        /// <remarks>Default is <see cref="DbProviderSelector"/>.</remarks>
        public Type DbProviderSelectorImplementation { get; set; } = typeof(DbProviderSelector);

        /// <summary>
        /// Gets or sets the <see cref="IDbProviderSelector"/><see cref="LifetimeScopeType"/>.
        /// </summary>
        /// <remarks>Default is <see cref="LifetimeScopeType.Singleton"/>.</remarks>
        public LifetimeScopeType DbProviderSelectorLifetimeScope { get; set; } = LifetimeScopeType.Singleton;

        /// <summary>
        /// Gets or sets the <see cref="DbProviderFactory"/> instances.
        /// </summary>
        /// <remarks>Default is <see cref="SqlClientFactory"/>.</remarks>
        public DbProviderFactory[] DbProviderFactoryInstances { get; set; } = new DbProviderFactory[] { SqlClientFactory.Instance };

        /// <summary>
        /// Gets or sets the <see cref="DbProviderFactory"/><see cref="LifetimeScopeType"/>.
        /// </summary>
        /// <remarks>Default is <see cref="LifetimeScopeType.Singleton"/>.</remarks>
        public LifetimeScopeType DbProviderFactoryLifetimeScope { get; set; } = LifetimeScopeType.Singleton;

        #endregion Public Properties

        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of <see cref="DatabaseServiceRegistration"/>.
        /// </summary>
        public DatabaseServiceRegistration()
            : base(null) { }

        /// <summary>
        /// Initializes a new instance of <see cref="DatabaseServiceRegistration"/>.
        /// </summary>
        /// <param name="supportAssemblies">The support assemblies.</param>
        public DatabaseServiceRegistration(IEnumerable<Assembly> supportAssemblies)
            : base(supportAssemblies) { }

        #endregion Public Constructors

        #region Public Override Methods

        /// <inheritdoc/>
        public override void Register() {
            Builder
                .RegisterType(GetDatabaseImplementation())
                .As<IDatabase>()
                .EnableInterfaceInterceptors()
                .SetLifetimeScope(DatabaseLifetimeScope);

            Builder
                .RegisterType(GetDbProviderSelectorImplementation())
                .As<IDbProviderSelector>()
                .SetLifetimeScope(DbProviderSelectorLifetimeScope);

            if (DbProviderFactoryInstances != null) {
                foreach (var dbProviderFactory in DbProviderFactoryInstances) {
                    Builder
                        .RegisterInstance(dbProviderFactory)
                        .Named<DbProviderFactory>(dbProviderFactory.GetType().FullName)
                        .SetLifetimeScope(DbProviderFactoryLifetimeScope);
                }
            }
        }

        #endregion Public Override Methods

        #region Private Methods

        private Type GetDatabaseImplementation() {
            return (DatabaseImplementation == null)
                ? GetImplementationsFromSupportAssemblies(typeof(IDatabase)).SingleOrDefault()
                : DatabaseImplementation;
        }

        private Type GetDbProviderSelectorImplementation() {
            return (DbProviderSelectorImplementation == null)
                ? GetImplementationsFromSupportAssemblies(typeof(IDbProviderSelector)).SingleOrDefault()
                : DbProviderSelectorImplementation;
        }

        #endregion Private Methods
    }
}