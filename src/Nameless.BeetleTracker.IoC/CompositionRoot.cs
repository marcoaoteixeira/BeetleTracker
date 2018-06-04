using System;
using Autofac;

namespace Nameless.BeetleTracker.IoC {

    /// <summary>
    /// Default implementation of <see cref="ICompositionRoot"/> using Autofac (https://autofac.org/).
    /// </summary>
    public class CompositionRoot : ICompositionRoot, IDisposable {

        #region Private Fields

        private bool _disposed;

        #endregion Private Fields

        #region Public Properties

        /// <summary>
        /// Gets the Autofac container.
        /// </summary>
        public IContainer Container { get; private set; }

        #endregion Public Properties

        #region Private Properties

        private ContainerBuilder Builder { get; set; }

        #endregion Private Properties

        #region Destructor

        /// <summary>
        /// Destructor.
        /// </summary>
        ~CompositionRoot() {
            Dispose(disposing: false);
        }

        #endregion Destructor

        #region Private Methods

        private ContainerBuilder GetBuilder() => Builder ?? (Builder = new ContainerBuilder());

        private void Dispose(bool disposing) {
            if (_disposed) { return; }
            if (disposing) { TearDown(); }
            _disposed = true;
        }

        #endregion Private Methods

        #region ICompositionRoot Members

        /// <inheritdoc/>
        public IResolver Resolver {
            get {
                if (Container == null) {
                    throw new InvalidOperationException(string.Format(Properties.Resources.CompositionRootNotStarted, nameof(StartUp)));
                }
                return Container.Resolve<IResolver>();
            }
        }

        /// <inheritdoc/>
        public void Compose(params IServiceRegistration[] registrations) {
            if (Container != null) {
                throw new InvalidOperationException(Properties.Resources.CompositionRootAlreadyStarted);
            }

            foreach (var registration in registrations) {
                var innerRegistration = registration as ServiceRegistrationBase;
                if (innerRegistration != null) {
                    innerRegistration.SetBuilder(GetBuilder());
                }
                registration.Register();
            }
        }

        /// <inheritdoc/>
        public void StartUp() {
            if (Container != null) {
                throw new InvalidOperationException(Properties.Resources.CompositionRootAlreadyStarted);
            }

            GetBuilder()
                .RegisterType<Resolver>()
                .As<IResolver>()
                .PreserveExistingDefaults();

            Container = GetBuilder().Build();
        }

        /// <inheritdoc/>
        public void TearDown() {
            if (Container != null) {
                Container.Dispose();
            }

            Container = null;
            Builder = null;
        }

        #endregion ICompositionRoot Members

        #region IDisposable Members

        /// <inheritdoc/>
        public void Dispose() {
            Dispose(disposing: true);
            GC.SuppressFinalize(obj: this);
        }

        #endregion IDisposable Members
    }
}