using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac;
using Autofac.Core;
using Autofac.Core.Activators.Reflection;
using Nameless.BeetleTracker.IoC;
using Nameless.BeetleTracker.Settings;

namespace Nameless.BeetleTracker.IoC.ServiceRegistrations {

    /// <summary>
    /// Autofac module for implementations from Nameless.WinFormsFramework.Settings namespace.
    /// </summary>
    public sealed class SettingsServiceRegistration : ServiceRegistrationBase {

        #region Public Properties

        /// <summary>
        /// Gets or sets the <see cref="ISettingsStorage"/> implementation.
        /// </summary>
        public Type SettingsStorageImplementation { get; set; } = typeof(JsonSettingsStorage);

        /// <summary>
        /// Gets or sets the <see cref="ISettingsStorage"/><see cref="LifetimeScopeType"/>.
        /// </summary>
        /// <remarks>Default is <see cref="LifetimeScopeType.Singleton"/>.</remarks>
        public LifetimeScopeType SettingsStorageLifetimeScope { get; set; } = LifetimeScopeType.Singleton;

        /// <summary>
        /// Gets or sets the <see cref="SettingsBase"/><see cref="LifetimeScopeType"/>.
        /// </summary>
        /// <remarks>Default is <see cref="LifetimeScopeType.Transient"/>.</remarks>
        public LifetimeScopeType SettingsBaseLifetimeScope { get; set; } = LifetimeScopeType.Transient;

        #endregion Public Properties

        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of <see cref="SettingsServiceRegistration"/>
        /// </summary>
        public SettingsServiceRegistration()
            : base(null) { }

        /// <summary>
        /// Initializes a new instance of <see cref="SettingsServiceRegistration"/>
        /// </summary>
        /// <param name="supportAssemblies">Support assemblies.</param>
        public SettingsServiceRegistration(IEnumerable<Assembly> supportAssemblies)
            : base(supportAssemblies) { }

        #endregion Public Constructors

        #region Public Override Methods

        /// <inheritdoc/>
        public override void Register() {
            Builder
                .RegisterModule(new InnerModule(GetSettingsStorageImplementation(), SettingsBaseLifetimeScope));
        }

        #endregion Public Override Methods

        #region Private Methods

        private Type GetSettingsStorageImplementation() {
            return SettingsStorageImplementation == null
                ? GetImplementationsFromSupportAssemblies(typeof(ISettingsStorage)).SingleOrDefault()
                : SettingsStorageImplementation;
        }

        #endregion Private Methods

        #region Private Classes

        private class InnerModule : Autofac.Module {

            #region Private Read-Only Fields

            private readonly Type _settingsStorageType;
            private readonly LifetimeScopeType _settingsStorageLifetimeScopeType;
            private readonly ConcurrentDictionary<string, SettingsBase> _cache = new ConcurrentDictionary<string, SettingsBase>();

            #endregion Private Read-Only Fields

            #region Public Constructors

            public InnerModule(Type settingsStorageType, LifetimeScopeType settingsStorageLifetimeScopeType) {
                _settingsStorageType = settingsStorageType;
                _settingsStorageLifetimeScopeType = settingsStorageLifetimeScopeType;
            }

            #endregion Public Constructors

            #region Protected Override Methods

            protected override void Load(ContainerBuilder builder) {
                builder
                    .RegisterType(_settingsStorageType)
                    .As<ISettingsStorage>()
                    .SetLifetimeScope(_settingsStorageLifetimeScopeType);
                base.Load(builder);
            }

            protected override void AttachToComponentRegistration(IComponentRegistry componentRegistry, IComponentRegistration registration) {
                var reflectionActivator = registration.Activator as ReflectionActivator;

                if (reflectionActivator != null) {
                    var target = reflectionActivator.LimitType;
                    var settingsTypes = reflectionActivator.ConstructorFinder.FindConstructors(target)
                        .SelectMany(_ => _.GetParameters().Where(parameter => typeof(SettingsBase).IsAssignableFrom(parameter.ParameterType)))
                        .DistinctBy(_ => _.ParameterType.FullName);

                    if (settingsTypes.Any()) {
                        registration.Preparing += (sender, args) => {
                            var parameters = settingsTypes.Select(_ => new TypedParameter(type: _.ParameterType, value: GetCachedSettings(_.ParameterType, args.Context)));
                            args.Parameters = args.Parameters.Concat(parameters.ToArray());
                        };
                    }
                }

                base.AttachToComponentRegistration(componentRegistry, registration);
            }

            #endregion Protected Override Methods

            #region Private Methods

            private SettingsBase GetCachedSettings(Type settingsType, IComponentContext context) => _cache.GetOrAdd(
                key: settingsType.ToString(),
                valueFactory: value => (SettingsBase)context.Resolve<ISettingsStorage>().Load(settingsType)
            );

            #endregion Private Methods
        }

        #endregion Private Classes
    }
}