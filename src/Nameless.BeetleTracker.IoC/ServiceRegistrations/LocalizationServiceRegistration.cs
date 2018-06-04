using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac;
using Autofac.Core;
using Nameless.BeetleTracker.IoC;
using Nameless.BeetleTracker.Localization;
using Nameless.BeetleTracker.Localization.Json;
using Nameless.BeetleTracker.Settings;

namespace Nameless.BeetleTracker.IoC.ServiceRegistrations {

    /// <summary>
    /// Autofac module implementation for Nameless.BeetleTracker.Localization namespace.
    /// </summary>
    public sealed class LocalizationServiceRegistration : ServiceRegistrationBase {

        #region Public Properties

        /// <summary>
        /// Gets or sets the <see cref="IPluralStringLocalizer{T}"/> implementation.
        /// </summary>
        /// <remarks>Default is <see cref="PluralStringLocalizer{T}"/>.</remarks>
        public Type PluralStringLocalizerImplementation { get; set; } = typeof(PluralStringLocalizer<>);

        /// <summary>
        /// Gets or sets the <see cref="IPluralStringLocalizer{T}"/><see cref="LifetimeScopeType"/>.
        /// </summary>
        /// <remarks>Default is <see cref="LifetimeScopeType.Transient"/>.</remarks>
        public LifetimeScopeType PluralStringLocalizerLifetimeScope { get; set; } = LifetimeScopeType.Transient;


        /// <summary>
        /// Gets or sets the <see cref="IStringLocalizerFactory"/> implementation.
        /// </summary>
        /// <remarks>Default is <see cref="FileSystemStringLocalizerFactory"/>.</remarks>
        public Type StringLocalizerFactoryImplementation { get; set; } = typeof(FileSystemStringLocalizerFactory);

        /// <summary>
        /// Gets or sets the <see cref="IStringLocalizerFactory"/><see cref="LifetimeScopeType"/>.
        /// </summary>
        /// <remarks>Default is <see cref="LifetimeScopeType.Singleton"/>.</remarks>
        public LifetimeScopeType StringLocalizerFactoryLifetimeScope { get; set; } = LifetimeScopeType.Singleton;

        #endregion Public Properties

        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of <see cref="LocalizationServiceRegistration"/>.
        /// </summary>
        public LocalizationServiceRegistration()
            : base(null) { }

        /// <summary>
        /// Initializes a new instance of <see cref="LocalizationServiceRegistration"/>.
        /// </summary>
        /// <param name="supportAssemblies">The support assemblies.</param>
        public LocalizationServiceRegistration(IEnumerable<Assembly> supportAssemblies)
            : base(supportAssemblies) { }

        #endregion Public Constructors

        #region Public Override Methods

        /// <inheritdoc/>
        public override void Register() {
            Builder
                .RegisterModule(new InnerModule(
                    GetPluralStringLocalizerImplementation(),
                    PluralStringLocalizerLifetimeScope,
                    GetStringLocalizerFactoryImplementation(),
                    StringLocalizerFactoryLifetimeScope
                ));
        }

        #endregion Public Override Methods

        #region Private Methods

        private Type GetPluralStringLocalizerImplementation() {
            return PluralStringLocalizerImplementation == null
                ? GetImplementationsFromSupportAssemblies(typeof(IPluralStringLocalizer<>)).SingleOrDefault()
                : PluralStringLocalizerImplementation;
        }

        private Type GetStringLocalizerFactoryImplementation() {
            return StringLocalizerFactoryImplementation == null
                ? GetImplementationsFromSupportAssemblies(typeof(IStringLocalizerFactory)).SingleOrDefault()
                : StringLocalizerFactoryImplementation;
        }

        #endregion Private Methods

        #region Private Inner Classes

        private class InnerModule : Autofac.Module {

            #region Private Static Read-Only Fields

            private static readonly ConcurrentDictionary<string, IStringLocalizer> Cache = new ConcurrentDictionary<string, IStringLocalizer>();

            #endregion Private Static Read-Only Fields

            #region Private Fields

            private Type _stringLocalizerFactoryImplementation;
            private LifetimeScopeType _stringLocalizerFactoryLifetimeScope;

            private Type _pluralStringLocalizerImplementation;
            private LifetimeScopeType _pluralStringLocalizerLifetimeScope;

            #endregion Private Fields

            #region Public Constructors

            public InnerModule(Type pluralStringLocalizerImplementation, LifetimeScopeType pluralStringLocalizerLifetimeScope, Type stringLocalizerFactoryImplementation, LifetimeScopeType stringLocalizerFactoryLifetimeScope) {
                _pluralStringLocalizerImplementation = pluralStringLocalizerImplementation;
                _pluralStringLocalizerLifetimeScope = pluralStringLocalizerLifetimeScope;

                _stringLocalizerFactoryImplementation = stringLocalizerFactoryImplementation;
                _stringLocalizerFactoryLifetimeScope = stringLocalizerFactoryLifetimeScope;
            }

            #endregion Public Constructors

            #region Protected Override Methods

            protected override void Load(ContainerBuilder builder) {
                builder
                    .RegisterTypes(new[] { _pluralStringLocalizerImplementation })
                    .AsClosedTypesOf(typeof(IPluralStringLocalizer<>))
                    .SetLifetimeScope(_pluralStringLocalizerLifetimeScope);

                builder
                    .RegisterType(_stringLocalizerFactoryImplementation)
                    .As<IStringLocalizerFactory>()
                    .OnPreparing(StringLocalizerFactoryPreparing)
                    .SetLifetimeScope(_stringLocalizerFactoryLifetimeScope);
                base.Load(builder);
            }

            protected override void AttachToComponentRegistration(IComponentRegistry componentRegistry, IComponentRegistration registration) {
                var userProperty = FindUserProperty(registration.Activator.LimitType);
                if (userProperty != null) {
                    var scope = registration.Activator.LimitType;
                    registration.Activated += (sender, e) => {
                        if (e.Instance.GetType() != scope) { return; }
                        var localizer = Cache.GetOrAdd(scope.FullName, key => ResolveLocalizer(e.Context, scope));
                        userProperty.SetValue(e.Instance, localizer, null);
                    };
                }
                base.AttachToComponentRegistration(componentRegistry, registration);
            }

            #endregion Protected Override Methods

            #region Private Static Methods

            private static PropertyInfo FindUserProperty(Type type) => type.GetProperty("T", typeof(IStringLocalizer));

            private static IStringLocalizer ResolveLocalizer(IComponentContext context, Type scope) => context.Resolve<IStringLocalizerFactory>().Create(scope);

            private static void StringLocalizerFactoryPreparing(PreparingEventArgs action) {
                var options = action.Context.Resolve<ISettingsStorage>().Load<LocalizationOptions>();
                action.Parameters = action.Parameters.Union(new[] {
                    new TypedParameter(typeof(LocalizationOptions), options)
                });
            }

            #endregion Private Static Methods
        }

        #endregion Private Inner Classes
    }
}