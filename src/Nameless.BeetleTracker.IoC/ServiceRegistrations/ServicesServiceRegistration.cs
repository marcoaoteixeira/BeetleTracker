using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac;
using Nameless.BeetleTracker.Services;

namespace Nameless.BeetleTracker.IoC.ServiceRegistrations {

    /// <summary>
    /// The Services service registration.
    /// </summary>
    public sealed class ServicesServiceRegistration : ServiceRegistrationBase {

        #region Public Properties

        /// <summary>
        /// Gets or sets the <see cref="IClock"/> implementation.
        /// </summary>
        /// <remarks>Default is <see cref="Clock"/>.</remarks>
        public Type ClockImplementation { get; set; } = typeof(Clock);

        /// <summary>
        /// Gets or sets the <see cref="IClock"/><see cref="LifetimeScopeType"/>.
        /// </summary>
        /// <remarks>Default is <see cref="LifetimeScopeType.Singleton"/>.</remarks>
        public LifetimeScopeType ClockLifetimeScope { get; set; } = LifetimeScopeType.Singleton;

        #endregion Public Properties

        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of <see cref="ServicesServiceRegistration"/>
        /// </summary>
        public ServicesServiceRegistration()
            : base(supportAssemblies: null) { }

        /// <summary>
        /// Initializes a new instance of <see cref="ServicesServiceRegistration"/>
        /// </summary>
        /// <param name="supportAssemblies">A collection of support assemblies.</param>
        public ServicesServiceRegistration(IEnumerable<Assembly> supportAssemblies)
            : base(supportAssemblies) { }

        #endregion Public Constructors

        #region Public Override Methods

        /// <inheritdoc/>
        public override void Register() {
            Builder
                .RegisterType(GetClockImplementation())
                .As<IClock>()
                .SetLifetimeScope(ClockLifetimeScope);
        }

        #endregion Public Override Methods

        #region Private Methods

        private Type GetClockImplementation() {
            return ClockImplementation == null
                ? GetImplementationsFromSupportAssemblies(typeof(IClock)).SingleOrDefault()
                : ClockImplementation;
        }

        #endregion Private Methods
    }
}