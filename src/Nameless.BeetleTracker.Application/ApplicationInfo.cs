using System;

namespace Nameless.BeetleTracker {

    /// <summary>
    /// Default implementation of <see cref="IApplicationInfo"/>
    /// </summary>
    public class ApplicationInfo : IApplicationInfo {

        #region Public Static Read-Only Fields

        /// <summary>
        /// Gets an anonymous application information object.
        /// </summary>
        public static readonly IApplicationInfo Anonymous = new ApplicationInfo(Guid.Empty, "Anonymous");

        #endregion Public Static Read-Only Fields

        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of <see cref="ApplicationInfo"/>.
        /// </summary>
        /// <param name="id">The application ID.</param>
        /// <param name="name">The application name.</param>
        public ApplicationInfo(Guid id, string name) {
            ID = id;
            TenantName = name;
        }

        #endregion Public Constructors

        #region IApplicationInfo Members

        /// <inheritdoc/>
        public Guid ID { get; }

        /// <inheritdoc/>
        public string TenantName { get; }

        #endregion IApplicationInfo Members
    }
}