using System.Web;
using System.Web.Mvc;
using Microsoft.Owin.Security;

namespace Nameless.BeetleTracker.Mvc.Results {

    /// <summary>
    /// <see cref="HttpUnauthorizedResult"/> implementation for challenge.
    /// </summary>
    public class ChallengeResult : HttpUnauthorizedResult {

        #region Public Static Read-Only Fields

        /// <summary>
        /// The XSRF key.
        /// </summary>
        public static readonly string XsrfKey = "XsrfID";

        #endregion Public Static Read-Only Fields

        #region Public Properties

        /// <summary>
        /// Gets or sets the login provider.
        /// </summary>
        public string LoginProvider { get; set; }

        /// <summary>
        /// Gets or sets the URI to redirect after challenged.
        /// </summary>
        public string RedirectUri { get; set; }

        /// <summary>
        /// Gets or sets the user ID.
        /// </summary>
        public string UserId { get; set; }

        #endregion Public Properties

        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of <see cref="ChallengeResult"/>.
        /// </summary>
        /// <param name="provider">The login provider.</param>
        /// <param name="redirectUri">The redirect URI.</param>
        public ChallengeResult(string provider, string redirectUri)
            : this(provider, redirectUri, null) {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="ChallengeResult"/>.
        /// </summary>
        /// <param name="provider">The login provider.</param>
        /// <param name="redirectUri">The redirect URI.</param>
        /// <param name="userId">The user ID.</param>
        public ChallengeResult(string provider, string redirectUri, string userId) {
            LoginProvider = provider;
            RedirectUri = redirectUri;
            UserId = userId;
        }

        #endregion Public Constructors

        #region Public Override Methods

        /// <inheritdoc />
        public override void ExecuteResult(ControllerContext context) {
            var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
            if (UserId != null) {
                properties.Dictionary[XsrfKey] = UserId;
            }
            context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
        }

        #endregion Public Override Methods
    }
}