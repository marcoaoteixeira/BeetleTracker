using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac;
using Nameless.BeetleTracker.Notification;

namespace Nameless.BeetleTracker.IoC.ServiceRegistrations {

    /// <summary>
    /// The Notifier service registration.
    /// </summary>
    public sealed class NotifierServiceRegistration : ServiceRegistrationBase {

        #region Public Properties

        /// <summary>
        /// Gets or sets the <see cref="INotifier"/> implementation.
        /// </summary>
        /// <remarks>Default is <see cref="Notifier"/>.</remarks>
        public Type NotifierImplementation { get; set; } = typeof(Notifier);

        /// <summary>
        /// Gets or sets the <see cref="INotifier"/><see cref="LifetimeScopeType"/>.
        /// </summary>
        /// <remarks>Default is <see cref="LifetimeScopeType.PerScope"/>.</remarks>
        public LifetimeScopeType NotifierLifetimeScope { get; set; } = LifetimeScopeType.PerScope;

        #endregion Public Properties

        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of <see cref="NotifierServiceRegistration"/>
        /// </summary>
        public NotifierServiceRegistration()
            : base(supportAssemblies: null) { }

        /// <summary>
        /// Initializes a new instance of <see cref="NotifierServiceRegistration"/>
        /// </summary>
        /// <param name="supportAssemblies">A collection of support assemblies.</param>
        public NotifierServiceRegistration(IEnumerable<Assembly> supportAssemblies)
            : base(supportAssemblies) { }

        #endregion Public Constructors

        #region Public Override Methods

        /// <inheritdoc/>
        public override void Register() {
            Builder
                .RegisterType(GetNotifierImplementation())
                .As<INotifier>()
                .SetLifetimeScope(NotifierLifetimeScope);
        }

        #endregion Public Override Methods

        #region Private Methods

        private Type GetNotifierImplementation() {
            return NotifierImplementation == null
                ? GetImplementationsFromSupportAssemblies(typeof(INotifier)).SingleOrDefault()
                : NotifierImplementation;
        }

        #endregion Private Methods
    }
}