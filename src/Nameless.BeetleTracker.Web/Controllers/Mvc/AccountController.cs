using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Nameless.BeetleTracker.Mvc;
using Nameless.BeetleTracker.Identity;
using Nameless.BeetleTracker.Models.Mvc.Account;
using Nameless.BeetleTracker.Resources;
using Nameless.BeetleTracker.Mvc.Results;

namespace Nameless.BeetleTracker.Controllers.Mvc {

    /// <summary>
    /// Controller for account related tasks.
    /// </summary>
    public class AccountController : MvcController {

        #region Private Fields

        private SignInManager<IdentityUser, string> _signInManager;
        private UserManager<IdentityUser> _userManager;

        #endregion Private Fields

        #region Private Properties

        private IAuthenticationManager AuthenticationManager {
            get { return HttpContext.GetOwinContext().Authentication; }
        }

        #endregion Private Properties

        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of <see cref="AccountController"/>
        /// </summary>
        /// <param name="userManager">An instance of <see cref="UserManager{TUser}"/>.</param>
        /// <param name="signInManager">An instance of <see cref="SignInManager{TUser, TKey}"/>.</param>
        public AccountController(IdentityUserManager userManager, SignInManager<IdentityUser, string> signInManager) {
            Prevent.ParameterNull(userManager, nameof(userManager));
            Prevent.ParameterNull(signInManager, nameof(signInManager));

            _userManager = userManager;
            _signInManager = signInManager;
        }

        #endregion Public Constructors

        #region Public Methods

        /// <summary>
        /// Access the login page.
        /// </summary>
        /// <param name="returnUrl">The return URL if user access a blocked resource.</param>
        /// <returns>The login page view.</returns>
        [HttpGet /* GET: /Account/Login */]
        public ActionResult Login(string returnUrl) {
            ViewBag.ReturnUrl = returnUrl;
            return View(new LoginViewModel());
        }

        /// <summary>
        /// Do the login action.
        /// </summary>
        /// <param name="model">The login view model.</param>
        /// <param name="returnUrl">The URL to return, after login.</param>
        /// <returns>
        /// Multiples actions: The login view, if the model state is invalid; The <paramref
        /// name="returnUrl"/>, if the user succeeds; The lock out view, if the user locks its login;
        /// or; Redirects to the <see cref="SendCode(string, bool)"/> action.
        /// </returns>
        [HttpPost /* POST: /Account/Login */]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl) {
            if (!ModelState.IsValid) { return View(model); }

            // This does count login failures towards account lockout To disable password failures to
            // trigger account lockout, change to shouldLockout: false
            var result = await _signInManager.PasswordSignInAsync(model.Email /* userName */, model.Password, model.RememberMe, shouldLockout: true);
            switch (result) {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);

                case SignInStatus.LockedOut:
                    return View("Lockout");

                case SignInStatus.RequiresVerification:
                    return RedirectToAction(nameof(SendCode), new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });

                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError(string.Empty, T["Invalid login attempt."].Value);
                    return View(model);
            }
        }

        /// <summary>
        /// Access the verify code view.
        /// </summary>
        /// <param name="provider">The provider.</param>
        /// <param name="returnUrl">The return URL.</param>
        /// <param name="rememberMe">The remember me flag.</param>
        /// <returns>The verify code view.</returns>
        [HttpGet /* GET: /Account/VerifyCode */]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe) {
            // Require that the user has already logged in via username/password or external login
            if (!await _signInManager.HasBeenVerifiedAsync()) {
                return View("Error");
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        /// <summary>
        /// Verifies the code for two factor authentication.
        /// </summary>
        /// <param name="model">The verify code view model.</param>
        /// <returns>
        /// Multiple results: Verify code view, if model state is invalid; Redirects to the return
        /// URL, if succeeds; Lockout view, if user locks his credentials; or; Verify code view, if
        /// code is invalid.
        /// </returns>
        [HttpPost /* POST: /Account/VerifyCode */]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model) {
            if (!ModelState.IsValid) {
                return View(model);
            }

            // The following code protects for brute force attacks against the two factor codes. If a
            // user enters incorrect codes for a specified amount of time then the user account will
            // be locked out for a specified amount of time. You can configure the account lockout
            // settings in StartUp.Auth.cs
            var result = await _signInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent: model.RememberMe, rememberBrowser: model.RememberBrowser);
            switch (result) {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);

                case SignInStatus.LockedOut:
                    return View("Lockout");

                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError(string.Empty, T["Invalid code."].Value);
                    return View(model);
            }
        }

        /// <summary>
        /// Access the register view.
        /// </summary>
        /// <returns>The register view.</returns>
        [HttpGet /* GET: /Account/Register */]
        public ActionResult Register() {
            return View();
        }

        /// <summary>
        /// Executes the user registration.
        /// </summary>
        /// <param name="model">The registration view model.</param>
        /// <returns>
        /// If registration succeeds, then send user to the confirm email view; otherwise; to the
        /// register view.
        /// </returns>
        [HttpPost /* POST: /Account/Register */]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model) {
            if (ModelState.IsValid) {
                var user = new IdentityUser {
                    UserName = model.Email, // We will use the e-mail as the username.
                    FullName = model.FullName,
                    Email = model.Email,
                    ProfilePicture = Resource.no_user_image.ToByteArray() // Set the default profile picture to a "empty" image.
                };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded) {
                    // Send an email with this link
                    var emailConfirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    var callbackUrl = Url.Action(nameof(ConfirmEmail), "Account", new { userId = user.Id, code = emailConfirmationToken }, protocol: Request.Url.Scheme);
                    await _userManager.SendEmailAsync(user.Id
                        , T["Confirm your account"].Value
                        , T["Please confirm your account by clicking <a href=\"{0}\">here</a>", callbackUrl].Value
                    );

                    return RedirectToAction(nameof(ConfirmRegistration), "Account");
                }
                ModelState.AddErrosFromIdentityResult(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        /// <summary>
        /// Confirms the e-mail authentication factor.
        /// </summary>
        /// <param name="userId">The user ID.</param>
        /// <param name="code">The authentication code.</param>
        /// <returns>
        /// If code is valid, redirects to the confirmed email view; otherwise; to the error view.
        /// </returns>
        [HttpGet /* GET: /Account/ConfirmEmail */]
        public async Task<ActionResult> ConfirmEmail(string userId, string code) {
            if (userId == null || code == null) {
                return View("Error");
            }
            var result = await _userManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmedEmail" : "Error");
        }

        /// <summary>
        /// Access the forgot password view.
        /// </summary>
        /// <returns><see cref="ActionResult"/> to the forgot password view.</returns>
        [HttpGet /* GET: /Account/ForgotPassword */]
        public ActionResult ForgotPassword() {
            return View();
        }

        /// <summary>
        /// Executes the process of "Forgot password".
        /// </summary>
        /// <param name="model">Forgot password view model.</param>
        /// <returns></returns>
        [HttpPost /* POST: /Account/ForgotPassword */]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model) {
            if (ModelState.IsValid) {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user.Id))) {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View("ForgotPasswordConfirmation");
                }

                // Send an email with this link
                var passwordResetToken = await _userManager.GeneratePasswordResetTokenAsync(user.Id);
                var callbackUrl = Url.Action(nameof(ResetPassword), "Account", new { userId = user.Id, code = passwordResetToken }, protocol: Request.Url.Scheme);
                await _userManager.SendEmailAsync(user.Id
                    , T["Reset Password"].Value
                    , T["Please reset your password by clicking <a href=\"{0}\">here</a>", callbackUrl].Value);
                return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        /// <summary>
        /// Access the forgot password confirmation view.
        /// </summary>
        /// <returns><see cref="ActionResult"/> to the forgot password confirmation view</returns>
        [HttpGet /* GET: /Account/ForgotPasswordConfirmation */]
        public ActionResult ForgotPasswordConfirmation() {
            return View();
        }

        /// <summary>
        /// Access the reset password view.
        /// </summary>
        /// <param name="code">The reset password confirmation code.</param>
        /// <returns><see cref="ActionResult"/> to the reset password view</returns>
        [HttpGet /* GET: /Account/ResetPassword */]
        public ActionResult ResetPassword(string code) {
            return View(code == null ? "Error" : null);
        }

        /// <summary>
        /// Executes the reset password action.
        /// </summary>
        /// <param name="model">Reset password view model.</param>
        /// <returns></returns>
        [HttpPost /* POST: /Account/ResetPassword */]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model) {
            if (!ModelState.IsValid) {
                return View(model);
            }
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null) {
                // Don't reveal that the user does not exist
                return RedirectToAction(nameof(ResetPasswordConfirmation), "Account");
            }
            var result = await _userManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded) {
                return RedirectToAction(nameof(ResetPasswordConfirmation), "Account");
            }
            ModelState.AddErrosFromIdentityResult(result);
            return View();
        }

        /// <summary>
        /// Access the reset password confirmation view.
        /// </summary>
        /// <returns><see cref="ActionResult"/> to the reset password confirmation view.</returns>
        [HttpGet /* GET: /Account/ResetPasswordConfirmation */]
        public ActionResult ResetPasswordConfirmation() {
            return View();
        }

        /// <summary>
        /// Executes the external login action.
        /// </summary>
        /// <param name="provider">The provider name.</param>
        /// <param name="returnUrl">The return URL.</param>
        /// <returns>A <see cref="ChallengeResult"/> to the external login.</returns>
        [HttpPost /* POST: /Account/ExternalLogin */]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl) {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action(nameof(ExternalLoginCallback), "Account", new { ReturnUrl = returnUrl }));
        }

        /// <summary>
        /// Access the send code view.
        /// </summary>
        /// <param name="returnUrl">The return URL.</param>
        /// <param name="rememberMe">The remember me flag.</param>
        /// <returns>
        /// Multiple results: If user could not be found, error view; Otherwise, send code view.
        /// </returns>
        [HttpGet /* GET: /Account/SendCode */]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe) {
            var userId = await _signInManager.GetVerifiedUserIdAsync();
            if (userId == null) {
                return View("Error");
            }
            var userFactors = await _userManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();

            return View(new SendCodeViewModel {
                Providers = factorOptions,
                ReturnUrl = returnUrl,
                RememberMe = rememberMe
            });
        }

        /// <summary>
        /// Executes the send code action.
        /// </summary>
        /// <param name="model">Send code view model.</param>
        /// <returns>
        /// Multiple results: If could not send code, error view; Otherwise, redirects to action <see
        /// cref="VerifyCode(string, string, bool)"/>
        /// </returns>
        [HttpPost /* POST: /Account/SendCode */]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model) {
            if (!ModelState.IsValid) {
                return View();
            }

            // Generate the token and send it
            if (!await _signInManager.SendTwoFactorCodeAsync(model.SelectedProvider)) {
                return View("Error");
            }

            return RedirectToAction(nameof(VerifyCode), new {
                Provider = model.SelectedProvider,
                ReturnUrl = model.ReturnUrl,
                RememberMe = model.RememberMe
            });
        }

        /// <summary>
        /// Access the external login functionallity
        /// </summary>
        /// <param name="returnUrl">The return URL.</param>
        /// <returns>
        /// Multiple results: If could not get login info, redirects to <see cref="Login(string)"/>
        /// action. If external sign in status is:
        /// Success: Redirects to the return URL.
        /// LockedOut: Access the lockout view.
        /// RequiresVerification: Redirects to the <see cref="SendCode(string, bool)"/> action.
        /// Failure: Shows the external login confirmation view.
        /// </returns>
        [HttpGet /* GET: /Account/ExternalLoginCallback */]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl) {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null) {
                return RedirectToAction(nameof(Login));
            }

            // Sign in the user with this external login provider if the user already has a login
            var result = await _signInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result) {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);

                case SignInStatus.LockedOut:
                    return View("Lockout");

                case SignInStatus.RequiresVerification:
                    return RedirectToAction(nameof(SendCode), new { ReturnUrl = returnUrl, RememberMe = false });

                case SignInStatus.Failure:
                default:
                    // If the user does not have an account, then prompt the user to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

        /// <summary>
        /// Executs the external login confirmation action.
        /// </summary>
        /// <param name="model">External login confirmation view model.</param>
        /// <param name="returnUrl">The return URL.</param>
        /// <returns>
        /// Multiple results: If the user is authenticated, then redirects to <see
        /// cref="ManageController.Index(ManageController.ManageMessageId?)"/> If the model state is
        /// valid, try get external login info; otherwise; access the external login confirmation
        /// view. If external login info could not be found, access the external login failure view;
        /// otherwise; creates an application user entry. If the creation fails, access the external
        /// login confirmation view and show the errors; otherwise; redirects to the return URL.
        /// </returns>
        [HttpPost /* POST: /Account/ExternalLoginConfirmation */]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl) {
            if (User.Identity.IsAuthenticated) {
                return RedirectToAction(nameof(ManageController.Index), "Manage");
            }

            if (ModelState.IsValid) {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null) {
                    return View("ExternalLoginFailure");
                }
                var user = new IdentityUser {
                    FullName = model.Email,
                    UserName = model.Email,
                    Email = model.Email
                };
                var result = await _userManager.CreateAsync(user);
                if (result.Succeeded) {
                    result = await _userManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded) {
                        await _signInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                ModelState.AddErrosFromIdentityResult(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        /// <summary>
        /// Log off action.
        /// </summary>
        /// <returns>Redirects to the <see cref="HomeController.Index"/> action.</returns>
        [HttpPost /* POST: /Account/LogOff */]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff() {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        /// <summary>
        /// Access the external login failure view.
        /// </summary>
        /// <returns><see cref="ActionResult"/> to external login failure view.</returns>
        [HttpGet /* GET: /Account/ExternalLoginFailure */]
        public ActionResult ExternalLoginFailure() {
            return View();
        }

        /// <summary>
        /// Shows the view for confirm registration e-mail sent.
        /// </summary>
        /// <returns><see cref="ActionResult"/> for confirm registration e-mail sent view.</returns>
        [HttpGet /* GET: /Account/ConfirmRegistration */]
        public ActionResult ConfirmRegistration() {
            return View();
        }

        #endregion Public Methods

        #region Protected Override Methods

        /// <inheritdoc/>
        protected override void Dispose(bool disposing) {
            if (disposing) {
                if (_userManager != null) {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null) {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #endregion Protected Override Methods

        #region Private Methods

        private ActionResult RedirectToLocal(string returnUrl) {
            if (Url.IsLocalUrl(returnUrl)) {
                return Redirect(returnUrl);
            }
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        #endregion Private Methods
    }
}