using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac;
using Autofac.Core;
using Nameless.BeetleTracker.Logging;

namespace Nameless.BeetleTracker.IoC.ServiceRegistrations {

    /// <summary>
    /// Logging service registration.
    /// </summary>
    public sealed class LoggingServiceRegistration : ServiceRegistrationBase {

        #region Public Override Methods

        /// <inheritdoc />
        public override void Register() {
            Builder.RegisterModule<LoggingModule>();
        }

        #endregion Public Override Methods

        #region Internal Classes

        internal class LoggingModule : Autofac.Module {

            #region Private Read-Only Fields

            private readonly ConcurrentDictionary<string, ILogger> _cache = new ConcurrentDictionary<string, ILogger>();

            #endregion Private Read-Only Fields

            #region Private Static Methods

            private static ILogger CreateLogger(IComponentContext context, IEnumerable<Parameter> parameters) {
                return context.Resolve<ILoggerFactory>().CreateLogger(parameters.TypedAs<Type>());
            }

            #endregion Private Static Methods

            #region Public Override Methods

            protected override void Load(ContainerBuilder builder) {
                builder.RegisterType<LoggerFactory>().As<ILoggerFactory>().SingleInstance();
                builder.Register(CreateLogger).As<ILogger>().InstancePerDependency();
            }

            #endregion Public Override Methods

            #region Protected Override Methods

            protected override void AttachToComponentRegistration(IComponentRegistry componentRegistry, IComponentRegistration registration) {
                var implementationType = registration.Activator.LimitType;

                // verify if the implementation type needs logger injection via constructor.
                var constructorNeedLogger = implementationType.GetConstructors()
                    .Any(constructor => constructor.GetParameters()
                        .Any(parameter => parameter.ParameterType == typeof(ILogger)));

                // if need, inject and return.
                if (constructorNeedLogger) {
                    registration.Preparing += (sender, args) => {
                        var logger = GetCachedLogger(implementationType, args.Context);
                        var parameter = new TypedParameter(typeof(ILogger), logger);

                        args.Parameters = args.Parameters.Concat(new[] { parameter });
                    };

                    return;
                }

                // build an array of actions on this type to assign loggers to member properties
                var propertyInjectorCollection = BuildPropertyInjectorCollection(implementationType).ToArray();

                // otherwise, whan an instance of this component is activated, inject the loggers on
                // the instance
                registration.Activated += (sender, e) => {
                    foreach (var injector in propertyInjectorCollection) {
                        injector(e.Context, e.Instance);
                    }
                };
            }

            #endregion Protected Override Methods

            #region Private Methods

            private IEnumerable<Action<IComponentContext, object>> BuildPropertyInjectorCollection(IReflect componentType) {
                // Look for settable properties of type "ILogger"
                var properties = componentType
                    .GetProperties(BindingFlags.SetProperty | BindingFlags.Public | BindingFlags.Instance)
                    .Select(property => new {
                        PropertyInfo = property,
                        property.PropertyType,
                        IndexParameters = property.GetIndexParameters().ToArray(),
                        Accessors = property.GetAccessors(false)
                    })
                    .Where(property => property.PropertyType == typeof(ILogger)) // must be a logger
                    .Where(property => property.IndexParameters.Length == 0) // must not be an indexer
                    .Where(property => property.Accessors.Length != 1 || property.Accessors[0].ReturnType == typeof(void)); //must have get/set, or only set

                // Return an array of actions that resolve a logger and assign the property
                foreach (var entry in properties) {
                    var property = entry.PropertyInfo;

                    yield return (context, instance) => {
                        var logger = GetCachedLogger(componentType, context);
                        property.SetValue(instance, logger, null);
                    };
                }
            }

            private ILogger GetCachedLogger(IReflect componentType, IComponentContext context) {
                return _cache.GetOrAdd(componentType.ToString(),
                    value => context.Resolve<ILogger>(new TypedParameter(typeof(Type), componentType)));
            }

            #endregion Private Methods
        }

        #endregion Internal Classes
    }
}