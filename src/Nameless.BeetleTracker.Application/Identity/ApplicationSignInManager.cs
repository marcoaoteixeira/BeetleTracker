using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

namespace Nameless.BeetleTracker.Identity {

    /// <summary>
    /// Just a wrapper
    /// </summary>
    public sealed class ApplicationSignInManager : SignInManager<IdentityUser, string> {

        #region Public Constructors

        public ApplicationSignInManager(IdentityUserManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager) { }

        #endregion Public Constructors
    }
}