namespace Nameless.BeetleTracker.Identity {

    /// <summary>
    /// Represents an user identity claim.
    /// </summary>
    public class IdentityUserClaim {

        #region Private Read-Only Fields

        private readonly string _id;

        #endregion Private Read-Only Fields

        #region Internal Constructors

        internal IdentityUserClaim(string id = null) {
            _id = id;
        }

        #endregion Internal Constructors

        #region Public Properties

        /// <summary>
        /// Gets the user claim ID.
        /// </summary>
        public string Id {
            get { return _id; }
        }

        /// <summary>
        /// Gets or sets the claim type.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the claim value.
        /// </summary>
        public string Value { get; set; }

        #endregion Public Properties
    }
}