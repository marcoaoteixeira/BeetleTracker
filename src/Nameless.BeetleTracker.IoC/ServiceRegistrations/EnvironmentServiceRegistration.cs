using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac;
using Autofac.Core;
using Nameless.BeetleTracker.Environment;
using Nameless.BeetleTracker.Settings;

namespace Nameless.BeetleTracker.IoC.ServiceRegistrations {

    /// <summary>
    /// The HostingEnvironment service registration.
    /// </summary>
    public sealed class EnvironmentServiceRegistration : ServiceRegistrationBase {

        #region Public Properties

        /// <summary>
        /// Gets or sets the <see cref="IHostingEnvironment"/> implementation.
        /// </summary>
        /// <remarks>Default is <see cref="HostingEnvironment"/>.</remarks>
        public Type HostingEnvironmentImplementation { get; set; } = typeof(HostingEnvironment);

        /// <summary>
        /// Gets or sets the <see cref="IHostingEnvironment"/><see cref="LifetimeScopeType"/>.
        /// </summary>
        /// <remarks>Default is <see cref="LifetimeScopeType.Singleton"/>.</remarks>
        public LifetimeScopeType HostingEnvironmentLifetimeScope { get; set; } = LifetimeScopeType.Singleton;

        #endregion Public Properties

        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of <see cref="EnvironmentServiceRegistration"/>
        /// </summary>
        public EnvironmentServiceRegistration()
            : base(supportAssemblies: null) { }

        /// <summary>
        /// Initializes a new instance of <see cref="EnvironmentServiceRegistration"/>
        /// </summary>
        /// <param name="supportAssemblies">A collection of support assemblies.</param>
        public EnvironmentServiceRegistration(IEnumerable<Assembly> supportAssemblies)
            : base(supportAssemblies) { }

        #endregion Public Constructors

        #region Public Override Methods

        /// <inheritdoc/>
        public override void Register() {
            Builder
                .RegisterType(GetHostingEnvironmentImplementation())
                .As<IHostingEnvironment>()
                .SetLifetimeScope(HostingEnvironmentLifetimeScope);
        }

        #endregion Public Override Methods

        #region Private Methods

        private Type GetHostingEnvironmentImplementation() {
            return HostingEnvironmentImplementation == null
                ? GetImplementationsFromSupportAssemblies(typeof(IHostingEnvironment)).SingleOrDefault()
                : HostingEnvironmentImplementation;
        }

        #endregion Private Methods
    }
}