using System;
using System.Collections.Generic;

namespace Nameless.BeetleTracker {

    /// <summary>
    /// User information.
    /// </summary>
    public interface IUserInfo {

        #region Properties

        /// <summary>
        /// Gets the user ID.
        /// </summary>
        Guid ID { get; }
        /// <summary>
        /// Gets the user full name.
        /// </summary>
        string FullName { get; }
        /// <summary>
        /// Gets the user e-mail.
        /// </summary>
        string Email { get; }
        /// <summary>
        /// Gets the user roles.
        /// </summary>
        string[] Roles { get; }
        /// <summary>
        /// Gets the user claims
        /// </summary>
        IDictionary<string, string> Claims { get; }
        /// <summary>
        /// Whether if the user is a system administrator or not.
        /// </summary>
        bool SysAdmin { get; }

        #endregion Properties
    }
}