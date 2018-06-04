using System;
using System.Collections;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Nameless.BeetleTracker.Dynamic;

namespace Nameless.BeetleTracker.Identity {

    /// <summary>
    /// Representing an identity user.
    /// </summary>
    public class IdentityUser : IUser {

        #region Private Read-Only Fields

        private readonly string _id;
        private readonly IDictionary _attributes = new Hashtable();

        #endregion Private Read-Only Fields

        #region Public Properties

        /// <summary>
        /// Gets or sets the user full name.
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// Gets or sets the access failed counter.
        /// </summary>
        public int AccessFailedCount { get; set; }

        /// <summary>
        /// Gets or sets the e-mail
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets if the e-mail was confirmed.
        /// </summary>
        public bool EmailConfirmed { get; set; }

        /// <summary>
        /// Gets or sets the ability to lockout the identity.
        /// </summary>
        public bool LockoutEnabled { get; set; }

        /// <summary>
        /// Gets or sets the lockout end date (UTC).
        /// </summary>
        public DateTime? LockoutEndDateUtc { get; set; }

        /// <summary>
        /// Gets or sets the password hash.
        /// </summary>
        public string PasswordHash { get; set; }

        /// <summary>
        /// Gets or sets the phone number.
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Gets or sets if the phone number was confirmed.
        /// </summary>
        public bool PhoneNumberConfirmed { get; set; }

        /// <summary>
        /// Gets or sets if the two factor authentication is enabled.
        /// </summary>
        public bool TwoFactorEnabled { get; set; }

        /// <summary>
        /// Gets or sets the security stamp.
        /// </summary>
        public string SecurityStamp { get; set; }

        /// <summary>
        /// Gets or sets the profile picture.
        /// </summary>
        public byte[] ProfilePicture { get; set; }

        /// <summary>
        /// Gets or sets the entity state.
        /// </summary>
        public EntityState State { get; set; }

        /// <summary>
        /// Gets or sets the identity user attributes
        /// </summary>
        public dynamic Attributes {
            get { return new HashtableDynamicObject(_attributes); }
        }

        #endregion Public Properties

        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of <see cref="IdentityUser"/>
        /// </summary>
        public IdentityUser() { }

        #endregion

        #region Internal Constructors

        internal IdentityUser(string id = null, IDictionary attributes = null) {
            _id = id;
            _attributes = attributes ?? new Hashtable();
        }

        #endregion Internal Constructors

        #region Public Methods

        /// <summary>
        /// Creates a claims identity (Task) with the givem parameters.
        /// </summary>
        /// <param name="manager">An instance of <see cref="UserManager{TUser}"/>.</param>
        /// <param name="authenticationType">The authentication type.</param>
        /// <returns>A task for <see cref="ClaimsIdentity"/> object.</returns>
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<IdentityUser> manager, string authenticationType = null) {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(user: this, authenticationType: authenticationType ?? DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        #endregion Public Methods

        #region IUser<string> Members

        /// <inheritdoc/>
        public string Id {
            get { return _id; }
        }

        /// <inheritdoc/>
        public string UserName { get; set; }

        #endregion IUser<string> Members
    }
}