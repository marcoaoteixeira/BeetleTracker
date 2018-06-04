using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Nameless.BeetleTracker.Identity;
using Nameless.BeetleTracker.Models.Mvc.Manage;
using Nameless.BeetleTracker.Mvc;
using Nameless.BeetleTracker.Mvc.Results;

namespace Nameless.BeetleTracker.Controllers.Mvc {
    [Authorize]
    public class ManageController : MvcController {
        #region Private Fields

        private SignInManager<IdentityUser, string> _signInManager;
        private UserManager<IdentityUser> _userManager;

        #endregion

        #region Public Enums

        public enum ManageMessageId {
            AddPhoneSuccess,
            ChangePasswordSuccess,
            SetTwoFactorSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            RemovePhoneSuccess,
            Error
        }

        #endregion

        #region Private Properties

        private IAuthenticationManager AuthenticationManager {
            get { return HttpContext.GetOwinContext().Authentication; }
        }

        #endregion

        #region Public Constructors

        public ManageController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser, string> signInManager) {
            Prevent.ParameterNull(userManager, nameof(userManager));
            Prevent.ParameterNull(signInManager, nameof(signInManager));

            _userManager = userManager;
            _signInManager = signInManager;
        }

        #endregion

        #region Public Methods

        [HttpGet /* GET: /Manage/Index */]
        public async Task<ActionResult> Index(ManageMessageId? message) {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? T["Your password has been changed."].Value
                    : message == ManageMessageId.SetPasswordSuccess ? T["Your password has been set."].Value
                    : message == ManageMessageId.SetTwoFactorSuccess ? T["Your two-factor authentication provider has been set."].Value
                    : message == ManageMessageId.Error ? T["An error has occurred."].Value
                    : message == ManageMessageId.AddPhoneSuccess ? T["Your phone number was added."].Value
                    : message == ManageMessageId.RemovePhoneSuccess ? T["Your phone number was removed."].Value
                    : string.Empty;

            var userId = User.Identity.GetUserId();
            var model = new IndexViewModel {
                HasPassword = HasPassword(),
                PhoneNumber = await _userManager.GetPhoneNumberAsync(userId),
                TwoFactor = await _userManager.GetTwoFactorEnabledAsync(userId),
                Logins = await _userManager.GetLoginsAsync(userId),
                BrowserRemembered = await AuthenticationManager.TwoFactorBrowserRememberedAsync(userId)
            };
            return View(model);
        }

        [HttpPost /* POST: /Manage/RemoveLogin */]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemoveLogin(string loginProvider, string providerKey) {
            ManageMessageId? message;
            var result = await _userManager.RemoveLoginAsync(User.Identity.GetUserId(), new UserLoginInfo(loginProvider, providerKey));
            if (result.Succeeded) {
                var user = await _userManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null) {
                    await _signInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                message = ManageMessageId.RemoveLoginSuccess;
            } else { message = ManageMessageId.Error; }

            return RedirectToAction("ManageLogins", new { Message = message });
        }

        [HttpGet /* GET: /Manage/AddPhoneNumber */]
        public ActionResult AddPhoneNumber() {
            return View();
        }

        [HttpPost /* POST: /Manage/AddPhoneNumber */]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddPhoneNumber(AddPhoneNumberViewModel model) {
            if (!ModelState.IsValid) {
                return View(model);
            }
            // Generate the token and send it
            var code = await _userManager.GenerateChangePhoneNumberTokenAsync(User.Identity.GetUserId(), model.Number);
            if (_userManager.SmsService != null) {
                var message = new IdentityMessage {
                    Destination = model.Number,
                    Body = T["Your security code is: {0}", code].Value
                };
                await _userManager.SmsService.SendAsync(message);
            }
            return RedirectToAction("VerifyPhoneNumber", new { PhoneNumber = model.Number });
        }

        [HttpPost /* POST: /Manage/EnableTwoFactorAuthentication */]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EnableTwoFactorAuthentication() {
            await _userManager.SetTwoFactorEnabledAsync(User.Identity.GetUserId(), true);
            var user = await _userManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null) {
                await _signInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Index", "Manage");
        }

        [HttpPost /* POST: /Manage/DisableTwoFactorAuthentication */]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DisableTwoFactorAuthentication() {
            await _userManager.SetTwoFactorEnabledAsync(User.Identity.GetUserId(), false);
            var user = await _userManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null) {
                await _signInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Index", "Manage");
        }

        [HttpGet /* GET: /Manage/VerifyPhoneNumber */]
        public async Task<ActionResult> VerifyPhoneNumber(string phoneNumber) {
            var code = await _userManager.GenerateChangePhoneNumberTokenAsync(User.Identity.GetUserId(), phoneNumber);
            // Send an SMS through the SMS provider to verify the phone number
            return phoneNumber != null
                ? View(new VerifyPhoneNumberViewModel { PhoneNumber = phoneNumber })
                : View("Error");
        }

        [HttpPost /* POST: /Manage/VerifyPhoneNumber */]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyPhoneNumber(VerifyPhoneNumberViewModel model) {
            if (!ModelState.IsValid) {
                return View(model);
            }
            var result = await _userManager.ChangePhoneNumberAsync(User.Identity.GetUserId(), model.PhoneNumber, model.Code);
            if (result.Succeeded) {
                var user = await _userManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null) {
                    await _signInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                return RedirectToAction("Index", new { Message = ManageMessageId.AddPhoneSuccess });
            }
            // If we got this far, something failed, redisplay form
            ModelState.AddModelError(string.Empty, T["Failed to verify phone"].Value);
            return View(model);
        }

        [HttpGet /* GET: /Manage/RemovePhoneNumber */]
        public async Task<ActionResult> RemovePhoneNumber() {
            var result = await _userManager.SetPhoneNumberAsync(User.Identity.GetUserId(), null);
            if (!result.Succeeded) {
                return RedirectToAction("Index", new { Message = ManageMessageId.Error });
            }
            var user = await _userManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null) {
                await _signInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Index", new { Message = ManageMessageId.RemovePhoneSuccess });
        }

        [HttpGet /* GET: /Manage/ChangePassword */]
        public ActionResult ChangePassword() {
            return View();
        }

        [HttpPost /* POST: /Manage/ChangePassword */]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model) {
            if (!ModelState.IsValid) {
                return View(model);
            }
            var result = await _userManager.ChangePasswordAsync(User.Identity.GetUserId(), model.CurrentPassword, model.NewPassword);
            if (result.Succeeded) {
                var user = await _userManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null) {
                    await _signInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                return RedirectToAction("Index", new { Message = ManageMessageId.ChangePasswordSuccess });
            }
            ModelState.AddErrosFromIdentityResult(result);
            return View(model);
        }

        [HttpGet /* GET: /Manage/SetPassword */]
        public ActionResult SetPassword() {
            return View();
        }

        [HttpPost /* POST: /Manage/SetPassword */]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SetPassword(SetPasswordViewModel model) {
            if (ModelState.IsValid) {
                var result = await _userManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);
                if (result.Succeeded) {
                    var user = await _userManager.FindByIdAsync(User.Identity.GetUserId());
                    if (user != null) {
                        await _signInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                    }
                    return RedirectToAction("Index", new { Message = ManageMessageId.SetPasswordSuccess });
                }
                ModelState.AddErrosFromIdentityResult(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpGet /* GET: /Manage/ManageLogins */]
        public async Task<ActionResult> ManageLogins(ManageMessageId? message) {
            ViewBag.StatusMessage =
                message == ManageMessageId.RemoveLoginSuccess ? T["The external login was removed."].Value
                    : message == ManageMessageId.Error ? T["An error has occurred."].Value
                    : string.Empty;
            var user = await _userManager.FindByIdAsync(User.Identity.GetUserId());
            if (user == null) {
                return View("Error");
            }
            var userLogins = await _userManager.GetLoginsAsync(User.Identity.GetUserId());
            var otherLogins = AuthenticationManager.GetExternalAuthenticationTypes().Where(auth => userLogins.All(ul => auth.AuthenticationType != ul.LoginProvider)).ToList();
            ViewBag.ShowRemoveButton = user.PasswordHash != null || userLogins.Count > 1;
            return View(new ManageLoginsViewModel {
                CurrentLogins = userLogins,
                OtherLogins = otherLogins
            });
        }

        [HttpPost /* POST: /Manage/LinkLogin */]
        [ValidateAntiForgeryToken]
        public ActionResult LinkLogin(string provider) {
            // Request a redirect to the external login provider to link a login for the current user
            return new ChallengeResult(provider, Url.Action("LinkLoginCallback", "Manage"), User.Identity.GetUserId());
        }

        [HttpGet /* GET: /Manage/LinkLoginCallback */]
        public async Task<ActionResult> LinkLoginCallback() {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync(ChallengeResult.XsrfKey, User.Identity.GetUserId());
            if (loginInfo == null) {
                return RedirectToAction("ManageLogins", new { Message = ManageMessageId.Error });
            }
            var result = await _userManager.AddLoginAsync(User.Identity.GetUserId(), loginInfo.Login);
            return !result.Succeeded
                ? RedirectToAction("ManageLogins", new { Message = ManageMessageId.Error })
                : RedirectToAction("ManageLogins");
        }

        #endregion

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

        #region Private Methods

        private bool HasPassword() {
            var user = _userManager.FindById(User.Identity.GetUserId());
            if (user != null) {
                return user.PasswordHash != null;
            }
            return false;
        }

        private bool HasPhoneNumber() {
            var user = _userManager.FindById(User.Identity.GetUserId());
            if (user != null) {
                return user.PhoneNumber != null;
            }
            return false;
        }

        #endregion
    }
}