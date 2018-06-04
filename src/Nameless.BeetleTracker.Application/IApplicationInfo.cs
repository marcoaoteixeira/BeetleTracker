using System;

namespace Nameless.BeetleTracker {
    /// <summary>
    /// Application information.
    /// </summary>
    public interface IApplicationInfo {
        #region Properties
        
        /// <summary>
        /// Gets the application ID.
        /// </summary>
        Guid ID { get; }
        /// <summary>
        /// Gets the application name.
        /// </summary>
        string TenantName { get; }

        #endregion
    }
}