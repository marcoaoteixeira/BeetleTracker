namespace Nameless.BeetleTracker.Identity {

    /// <summary>
    /// Represents an user identity login.
    /// </summary>
    public class IdentityUserLogin {

        #region Public Properties

        /// <summary>
        /// Gets or sets the login provider.
        /// </summary>
        public string LoginProvider { get; set; }

        /// <summary>
        /// Gets or sets the provider key.
        /// </summary>
        public string ProviderKey { get; set; }

        #endregion Public Properties
    }
}