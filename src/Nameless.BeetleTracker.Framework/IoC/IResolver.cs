using System;

namespace Nameless.BeetleTracker.IoC {

    /// <summary>
    /// Resolver interface.
    /// </summary>
    public interface IResolver {

        #region Methods

        /// <summary>
        /// Resolves a service by its type.
        /// </summary>
        /// <param name="serviceType">The service type.</param>
        /// <param name="name">The service name. If any.</param>
        /// <returns>The instance of the service.</returns>
        object Resolve(Type serviceType, string name = null);

        #endregion Methods
    }
}