using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac;

namespace Nameless.BeetleTracker.IoC {

    /// <summary>
    /// Abstract implementation of <see cref="IServiceRegistration"/> for Autofac (https://autofac.org/).
    /// </summary>
    public abstract class ServiceRegistrationBase : IServiceRegistration {

        #region Protected Properties

        /// <summary>
        /// Gets the current <see cref="ContainerBuilder"/> instance.
        /// </summary>
        protected ContainerBuilder Builder { get; private set; }

        /// <summary>
        /// Gets the support assemblies.
        /// </summary>
        protected Assembly[] SupportAssemblies { get; }

        #endregion Protected Properties

        #region Protected Constructors

        /// <summary>
        /// Protected constructor.
        /// </summary>
        protected ServiceRegistrationBase()
            : this(Enumerable.Empty<Assembly>()) { }

        /// <summary>
        /// Protected constructor.
        /// </summary>
        /// <param name="supportAssemblies">The support assemblies.</param>
        protected ServiceRegistrationBase(IEnumerable<Assembly> supportAssemblies) {
            SupportAssemblies = supportAssemblies != null ? supportAssemblies.ToArray() : Array.Empty<Assembly>();
        }

        #endregion Protected Constructors

        #region Public Methods

        /// <summary>
        /// Sets the current container builder instance.
        /// </summary>
        /// <param name="builder">The container builder.</param>
        public void SetBuilder(ContainerBuilder builder) {
            Prevent.ParameterNull(builder, nameof(builder));

            Builder = builder;
        }

        #endregion Public Methods

        #region Protected Methods

        /// <summary>
        /// Retrieves all implementations for a given service type.
        /// </summary>
        /// <param name="serviceType"></param>
        /// <returns></returns>
        protected Type[] GetImplementationsFromSupportAssemblies(Type serviceType) {
            if (!SupportAssemblies.Any()) { return Enumerable.Empty<Type>().ToArray(); }

            var result = SupportAssemblies
                .SelectMany(assembly => assembly.GetExportedTypes())
                .Where(type => !type.GetTypeInfo().IsAbstract && !type.GetTypeInfo().IsInterface)
                .Where(type => serviceType.IsAssignableFrom(type) || type.IsAssignableFromGenericType(serviceType))
                .ToArray();

            return result;
        }

        #endregion Protected Methods

        #region IServiceRegistration Members

        /// <inheritdoc />
        /// <remarks>You must never call <see cref="ContainerBuilder.Build(Autofac.Builder.ContainerBuildOptions)"/> in the <see cref="Register"/> method.</remarks>
        public abstract void Register();

        #endregion IServiceRegistration Members
    }
}