using System;
using System.Data.Common;

namespace Nameless.BeetleTracker.Data {
    /// <summary>
    /// Defines methods/properties/events to implement a DB factory provider.
    /// </summary>
    public interface IDbProviderSelector {

        #region Methods

        /// <summary>
        /// Retrieves a <see cref="DbProviderFactory"/> from a specific type.
        /// </summary>
        /// <param name="providerFactoryType">Type of the DB provider factory.</param>
        /// <returns>An instance of the specified type.</returns>
        DbProviderFactory GetFactory(Type providerFactoryType);

        /// <summary>
        /// Retrieves a <see cref="DbProviderFactory"/> from a specific type by its name.
        /// </summary>
        /// <param name="providerFactoryName">The name of the DB provider factory.</param>
        /// <returns>An instance of the specified type.</returns>
        DbProviderFactory GetFactory(string providerFactoryName);

        #endregion Methods
    }
}