using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac;
using Nameless.BeetleTracker.ErrorHandling;

namespace Nameless.BeetleTracker.IoC.ServiceRegistrations {

    /// <summary>
    /// Autofac module implementation for Nameless.BeetleTracker.ErrorHandling namespace.
    /// </summary>
    public class ErrorHandlingServiceRegistration : ServiceRegistrationBase {

        #region Public Properties

        /// <summary>
        /// Gets or sets the <see cref="IExceptionPolicy"/> implementation.
        /// </summary>
        /// <remarks>Default is <see cref="ExceptionPolicy"/>.</remarks>
        public Type ExceptionPolicyImplementation { get; set; } = typeof(ExceptionPolicy);

        /// <summary>
        /// Gets or sets the <see cref="IExceptionPolicy"/><see cref="LifetimeScopeType"/>.
        /// </summary>
        /// <remarks>Default is <see cref="LifetimeScopeType.Singleton"/>.</remarks>
        public LifetimeScopeType ExceptionPolicyLifetimeScope { get; set; } = LifetimeScopeType.Singleton;

        #endregion Public Properties

        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of <see cref="ErrorHandlingServiceRegistration"/>.
        /// </summary>
        public ErrorHandlingServiceRegistration()
            : base(null) { }

        /// <summary>
        /// Initializes a new instance of <see cref="ErrorHandlingServiceRegistration"/>.
        /// </summary>
        /// <param name="supportAssemblies">The support assemblies.</param>
        public ErrorHandlingServiceRegistration(IEnumerable<Assembly> supportAssemblies)
            : base(supportAssemblies) { }

        #endregion Public Constructors

        #region Public Override Methods

        /// <inheritdoc/>
        public override void Register() {
            Builder
                .RegisterType(GetExceptionPolicyImplementation())
                .As<IExceptionPolicy>()
                .SetLifetimeScope(ExceptionPolicyLifetimeScope);
        }

        #endregion Public Override Methods

        #region Private Methods

        private Type GetExceptionPolicyImplementation() {
            return ExceptionPolicyImplementation == null
                ? GetImplementationsFromSupportAssemblies(typeof(IExceptionPolicy)).SingleOrDefault()
                : ExceptionPolicyImplementation;
        }

        #endregion Private Methods
    }
}