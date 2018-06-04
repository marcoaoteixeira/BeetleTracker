using System;
using System.Collections.Generic;

namespace Nameless.BeetleTracker {
    /// <summary>
    /// Implementation of <see cref="IUserInfo"/>
    /// </summary>
    public class UserInfo : IUserInfo {

        #region Public Static Read-Only Fields

        /// <summary>
        /// Gets an anonymous user information object.
        /// </summary>
        public static readonly IUserInfo Anonymous = new UserInfo(Guid.Empty, "Anonymous", "anonymous@anonymous.com", new string[0], new Dictionary<string, string>(), false);

        #endregion Public Static Read-Only Fields

        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of <see cref="UserInfo"/>.
        /// </summary>
        /// <param name="id">The user ID.</param>
        /// <param name="fullName">The user full name.</param>
        /// <param name="email">The user e-mail.</param>
        /// <param name="roles">The user roles.</param>
        /// <param name="claims">The user claims.</param>
        /// <param name="sysAdmin">Whether the user is a system administrator.</param>
        public UserInfo(Guid id, string fullName, string email, string[] roles, IDictionary<string, string> claims, bool sysAdmin) {
            ID = id;
            FullName = fullName;
            Email = email;
            Roles = roles;
            Claims = claims;
            SysAdmin = sysAdmin;
        }

        #endregion Public Constructors

        #region IApplicationInfo Members

        /// <inheritdoc/>
        public Guid ID { get; }

        /// <inheritdoc/>
        public string FullName { get; }

        /// <inheritdoc/>
        public string Email { get; }

        /// <inheritdoc/>
        public string[] Roles { get; }

        /// <inheritdoc/>
        public IDictionary<string, string> Claims { get; }

        /// <inheritdoc/>
        public bool SysAdmin { get; }

        #endregion IApplicationInfo Members
    }
}