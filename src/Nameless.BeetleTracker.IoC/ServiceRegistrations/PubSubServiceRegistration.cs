using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac;
using Nameless.BeetleTracker.PubSub;

namespace Nameless.BeetleTracker.IoC.ServiceRegistrations {

    /// <summary>
    /// The Publisher/Subscriber service registration.
    /// </summary>
    public sealed class PubSubServiceRegistration : ServiceRegistrationBase {

        #region Public Properties

        /// <summary>
        /// Gets or sets the <see cref="IPublisherSubscriber"/> implementation.
        /// </summary>
        /// <remarks>Default is <see cref="PublisherSubscriber"/>.</remarks>
        public Type PublisherSubscriberImplementation { get; set; } = typeof(PublisherSubscriber);

        /// <summary>
        /// Gets or sets the <see cref="IPublisherSubscriber"/><see cref="LifetimeScopeType"/>.
        /// </summary>
        /// <remarks>Default is <see cref="LifetimeScopeType.Singleton"/>.</remarks>
        public LifetimeScopeType PublisherSubscriberLifetimeScope { get; set; } = LifetimeScopeType.Singleton;

        #endregion Public Properties

        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of <see cref="PubSubServiceRegistration"/>
        /// </summary>
        public PubSubServiceRegistration()
            : base(supportAssemblies: null) { }

        /// <summary>
        /// Initializes a new instance of <see cref="PubSubServiceRegistration"/>
        /// </summary>
        /// <param name="supportAssemblies">A collection of support assemblies.</param>
        public PubSubServiceRegistration(IEnumerable<Assembly> supportAssemblies)
            : base(supportAssemblies) { }

        #endregion Public Constructors

        #region Public Override Methods

        /// <inheritdoc/>
        public override void Register() {
            Builder
                .RegisterType(GetPublisherSubscriberImplementation())
                .As<IPublisherSubscriber>()
                .SetLifetimeScope(PublisherSubscriberLifetimeScope);
        }

        #endregion Public Override Methods

        #region Private Methods

        private Type GetPublisherSubscriberImplementation() {
            return PublisherSubscriberImplementation == null
                ? GetImplementationsFromSupportAssemblies(typeof(IPublisherSubscriber)).SingleOrDefault()
                : PublisherSubscriberImplementation;
        }

        #endregion Private Methods
    }
}