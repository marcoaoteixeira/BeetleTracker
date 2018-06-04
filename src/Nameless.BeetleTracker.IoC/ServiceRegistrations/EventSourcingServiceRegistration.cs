using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac;
using Nameless.BeetleTracker.EventSourcing.Bus;
using Nameless.BeetleTracker.EventSourcing.Domains;
using Nameless.BeetleTracker.EventSourcing.Events;
using Nameless.BeetleTracker.EventSourcing.Snapshots;
using Nameless.BeetleTracker.IoC;

namespace Nameless.BeetleTracker.IoC.ServiceRegistrations {

    /// <summary>
    /// Autofac module implementation for Nameless.BeetleTracker.EventStore assembly.
    /// </summary>
    public sealed class EventSourcingServiceRegistration : ServiceRegistrationBase {

        #region Public Properties

        /// <summary>
        /// Gets or sets the <see cref="IAggregateFactory"/> implementation.
        /// </summary>
        /// <remarks>Default is <see cref="AggregateFactory"/>.</remarks>
        public Type AggregateFactoryImplementation { get; set; } = typeof(AggregateFactory);

        /// <summary>
        /// Gets or sets the <see cref="IAggregateFactory"/><see cref="LifetimeScopeType"/>.
        /// </summary>
        /// <remarks>Default is <see cref="LifetimeScopeType.Singleton"/>.</remarks>
        public LifetimeScopeType AggregateFactoryLifetimeScope { get; set; } = LifetimeScopeType.Singleton;

        /// <summary>
        /// Gets or sets the <see cref="IBus"/> implementation.
        /// </summary>
        /// <remarks>Default is <see cref="DefaultBus"/>.</remarks>
        public Type BusImplementation { get; set; } = typeof(DefaultBus);

        /// <summary>
        /// Gets or sets the <see cref="IBus"/><see cref="LifetimeScopeType"/>.
        /// </summary>
        /// <remarks>Default is <see cref="LifetimeScopeType.Singleton"/>.</remarks>
        public LifetimeScopeType BusLifetimeScope { get; set; } = LifetimeScopeType.Singleton;

        /// <summary>
        /// Gets or sets the implementations of <see cref="IEventHandler{TEvent}"/>.
        /// </summary>
        public Type[] EventHandlerImplementations { get; set; } = Type.EmptyTypes;

        /// <summary>
        /// Gets or sets the <see cref="IEventHandler{TEvent}"/><see cref="LifetimeScopeType"/>.
        /// </summary>
        /// <remarks>Default is <see cref="LifetimeScopeType.Transient"/>.</remarks>
        public LifetimeScopeType EventHandlerLifetimeScope { get; set; } = LifetimeScopeType.Transient;

        /// <summary>
        /// Gets or sets the <see cref="ISession"/> implementation.
        /// </summary>
        /// <remarks>Default is <see cref="Session"/>.</remarks>
        public Type SessionImplementation { get; set; } = typeof(Session);

        /// <summary>
        /// Gets or sets the <see cref="ISession"/><see cref="LifetimeScopeType"/>.
        /// </summary>
        /// <remarks>Default is <see cref="LifetimeScopeType.PerScope"/>.</remarks>
        public LifetimeScopeType SessionLifetimeScope { get; set; } = LifetimeScopeType.PerScope;

        /// <summary>
        /// Gets or sets the <see cref="IRepository"/> implementation.
        /// </summary>
        /// <remarks>Default is <see cref="Repository"/>.</remarks>
        public Type RepositoryImplementation { get; set; } = typeof(Repository);

        /// <summary>
        /// Gets or sets the <see cref="IRepository"/><see cref="LifetimeScopeType"/>.
        /// </summary>
        /// <remarks>Default is <see cref="LifetimeScopeType.Singleton"/>.</remarks>
        public LifetimeScopeType RepositoryLifetimeScope { get; set; } = LifetimeScopeType.Singleton;

        /// <summary>
        /// Gets or sets the <see cref="IEventStore"/> implementation.
        /// </summary>
        /// <remarks>Default is <see cref="EventStore"/>.</remarks>
        public Type EventStoreImplementation { get; set; } = typeof(EventStore);

        /// <summary>
        /// Gets or sets the <see cref="IEventStore"/><see cref="LifetimeScopeType"/>.
        /// </summary>
        /// <remarks>Default is <see cref="LifetimeScopeType.Singleton"/>.</remarks>
        public LifetimeScopeType EventStoreLifetimeScope { get; set; } = LifetimeScopeType.Singleton;

        /// <summary>
        /// Gets or sets the <see cref="ISnapshotStore"/> implementation.
        /// </summary>
        /// <remarks>Default is <see cref="SnapshotStore"/>.</remarks>
        public Type SnapshotStoreImplementation { get; set; } = typeof(SnapshotStore);

        /// <summary>
        /// Gets or sets the <see cref="ISnapshotStore"/><see cref="LifetimeScopeType"/>.
        /// </summary>
        /// <remarks>Default is <see cref="LifetimeScopeType.Singleton"/>.</remarks>
        public LifetimeScopeType SnapshotStoreLifetimeScope { get; set; } = LifetimeScopeType.Singleton;

        /// <summary>
        /// Gets or sets the <see cref="ISnapshotStrategy"/> implementation.
        /// </summary>
        /// <remarks>Default is <see cref="SnapshotStrategy"/>.</remarks>
        public Type SnapshotStrategyImplementation { get; set; } = typeof(SnapshotStrategy);

        /// <summary>
        /// Gets or sets the <see cref="ISnapshotStrategy"/><see cref="LifetimeScopeType"/>.
        /// </summary>
        /// <remarks>Default is <see cref="LifetimeScopeType.Singleton"/>.</remarks>
        public LifetimeScopeType SnapshotStrategyLifetimeScope { get; set; } = LifetimeScopeType.Singleton;

        /// <summary>
        /// Whether should use the snapshot infrastructure, or not.
        /// </summary>
        public bool UseSnapshotStore { get; set; } = false;

        #endregion Public Properties

        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of <see cref="EventSourcingServiceRegistration"/>.
        /// </summary>
        public EventSourcingServiceRegistration()
            : base(null) { }

        /// <summary>
        /// Initializes a new instance of <see cref="EventSourcingServiceRegistration"/>.
        /// </summary>
        /// <param name="supportAssemblies">The support assemblies.</param>
        public EventSourcingServiceRegistration(IEnumerable<Assembly> supportAssemblies)
            : base(supportAssemblies) { }

        #endregion Public Constructors

        #region Public Override Methods

        /// <inheritdoc/>
        public override void Register() {
            Builder.RegisterType(GetAggregateFactoryImplementation()).As<IAggregateFactory>().SetLifetimeScope(BusLifetimeScope);
            Builder.RegisterType(GetBusImplementation()).AsImplementedInterfaces().SetLifetimeScope(BusLifetimeScope);
            Builder.RegisterTypes(GetEventHandlerImplementations()).AsClosedTypesOf(typeof(IEventHandler<>)).AsSelf().SetLifetimeScope(EventHandlerLifetimeScope);

            Builder.RegisterType(GetSessionImplementation()).As<ISession>().SetLifetimeScope(SessionLifetimeScope);
            Builder.RegisterType(GetRepositoryImplementation()).As<IRepository>().SetLifetimeScope(RepositoryLifetimeScope);

            Builder.RegisterType(GetEventStoreImplementation()).As<IEventStore>().SetLifetimeScope(EventStoreLifetimeScope);

            if (UseSnapshotStore) {
                Builder.RegisterType(GetSnapshotStoreImplementation()).As<ISnapshotStore>().SetLifetimeScope(SnapshotStoreLifetimeScope);

                Builder.RegisterType(GetSnapshotStrategyImplementation()).As<ISnapshotStrategy>().SetLifetimeScope(SnapshotStrategyLifetimeScope);
                Builder.RegisterDecorator<IRepository>((ctx, inner) => new SnapshotRepository(
                    aggregateFactory: ctx.Resolve<IAggregateFactory>(),
                    eventStore: ctx.Resolve<IEventStore>(),
                    repository: inner,
                    snapshotStore: ctx.Resolve<ISnapshotStore>(),
                    snapshotStrategy: ctx.Resolve<ISnapshotStrategy>()
                ), fromKey: "repository");
            }
        }

        #endregion Public Override Methods

        #region Private Methods

        private Type GetAggregateFactoryImplementation() {
            if (AggregateFactoryImplementation != null) {
                return AggregateFactoryImplementation;
            }

            return GetImplementationsFromSupportAssemblies(typeof(IAggregateFactory)).SingleOrDefault();
        }

        private Type GetBusImplementation() {
            if (BusImplementation != null) {
                return BusImplementation;
            }

            return GetImplementationsFromSupportAssemblies(typeof(IBus)).SingleOrDefault();
        }

        private Type[] GetEventHandlerImplementations() {
            if (EventHandlerImplementations.Any()) {
                return EventHandlerImplementations;
            }

            return GetImplementationsFromSupportAssemblies(typeof(IEventHandler<>));
        }

        private Type GetSessionImplementation() {
            if (SessionImplementation != null) {
                return SessionImplementation;
            }

            return GetImplementationsFromSupportAssemblies(typeof(ISession)).SingleOrDefault();
        }

        private Type GetRepositoryImplementation() {
            if (RepositoryImplementation != null) {
                return RepositoryImplementation;
            }

            return GetImplementationsFromSupportAssemblies(typeof(IRepository)).SingleOrDefault();
        }

        private Type GetEventStoreImplementation() {
            if (EventStoreImplementation != null) {
                return EventStoreImplementation;
            }

            return GetImplementationsFromSupportAssemblies(typeof(IEventStore)).SingleOrDefault();
        }

        private Type GetSnapshotStoreImplementation() {
            if (SnapshotStoreImplementation != null) {
                return SnapshotStoreImplementation;
            }

            return GetImplementationsFromSupportAssemblies(typeof(ISnapshotStore)).SingleOrDefault();
        }

        private Type GetSnapshotStrategyImplementation() {
            if (SnapshotStrategyImplementation != null) {
                return SnapshotStrategyImplementation;
            }

            return GetImplementationsFromSupportAssemblies(typeof(ISnapshotStrategy)).SingleOrDefault();
        }

        #endregion Private Methods
    }
}