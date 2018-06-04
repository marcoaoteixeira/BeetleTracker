using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac;
using Nameless.BeetleTracker.Text;

namespace Nameless.BeetleTracker.IoC.ServiceRegistrations {

    /// <summary>
    /// The Text service registration.
    /// </summary>
    public sealed class TextServiceRegistration : ServiceRegistrationBase {

        #region Public Properties

        /// <summary>
        /// Gets or sets the <see cref="IInterpolator"/> implementation.
        /// </summary>
        /// <remarks>Default is <see cref="Interpolator"/>.</remarks>
        public Type InterpolatorImplementation { get; set; } = typeof(Interpolator);

        /// <summary>
        /// Gets or sets the <see cref="IInterpolator"/><see cref="LifetimeScopeType"/>.
        /// </summary>
        /// <remarks>Default is <see cref="LifetimeScopeType.Singleton"/>.</remarks>
        public LifetimeScopeType InterpolatorLifetimeScope { get; set; } = LifetimeScopeType.Singleton;

        /// <summary>
        /// Gets or sets the <see cref="IDataBinder"/> implementation.
        /// </summary>
        /// <remarks>Default is <see cref="DataBinder"/>.</remarks>
        public Type DataBinderImplementation { get; set; } = typeof(DataBinder);

        /// <summary>
        /// Gets or sets the <see cref="IDataBinder"/><see cref="LifetimeScopeType"/>.
        /// </summary>
        /// <remarks>Default is <see cref="LifetimeScopeType.Singleton"/>.</remarks>
        public LifetimeScopeType DataBinderLifetimeScope { get; set; } = LifetimeScopeType.Singleton;

        #endregion Public Properties

        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of <see cref="TextServiceRegistration"/>
        /// </summary>
        public TextServiceRegistration()
            : base(supportAssemblies: null) { }

        /// <summary>
        /// Initializes a new instance of <see cref="TextServiceRegistration"/>
        /// </summary>
        /// <param name="supportAssemblies">A collection of support assemblies.</param>
        public TextServiceRegistration(IEnumerable<Assembly> supportAssemblies)
            : base(supportAssemblies) { }

        #endregion Public Constructors

        #region Public Override Methods

        /// <inheritdoc/>
        public override void Register() {
            Builder
                .RegisterType(GetInterpolatorImplementation())
                .As<IInterpolator>()
                .SetLifetimeScope(InterpolatorLifetimeScope);

            Builder
                .RegisterType(GetDataBinderImplementation())
                .As<IDataBinder>()
                .SetLifetimeScope(DataBinderLifetimeScope);
        }

        #endregion Public Override Methods

        #region Private Methods

        private Type GetInterpolatorImplementation() {
            return InterpolatorImplementation == null
                ? GetImplementationsFromSupportAssemblies(typeof(IInterpolator)).SingleOrDefault()
                : InterpolatorImplementation;
        }

        private Type GetDataBinderImplementation() {
            return DataBinderImplementation == null
                ? GetImplementationsFromSupportAssemblies(typeof(IDataBinder)).SingleOrDefault()
                : DataBinderImplementation;
        }

        #endregion Private Methods
    }
}