using System;
using Autofac;

namespace Nameless.BeetleTracker.IoC {

    /// <summary>
    /// Default implementation of <see cref="IResolver"/> using Autofac (https://autofac.org/).
    /// </summary>
    public sealed class Resolver : IResolver {

        #region Private Read-Only Fields

        private readonly ILifetimeScope _scope;

        #endregion Private Read-Only Fields

        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of <see cref="Resolver"/>.
        /// </summary>
        /// <param name="scope">The lifetime scope.</param>
        public Resolver(ILifetimeScope scope) {
            Prevent.ParameterNull(scope, nameof(scope));

            _scope = scope;
        }

        #endregion Public Constructors

        #region IResolver Members

        /// <inheritdoc />
        public object Resolve(Type serviceType, string name = null) {
            Prevent.ParameterNull(serviceType, nameof(serviceType));

            return !string.IsNullOrWhiteSpace(name)
                ? _scope.ResolveNamed(name, serviceType)
                : _scope.Resolve(serviceType);
        }

        #endregion IResolver Members
    }
}