using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using Microsoft.Web.Http;
using Nameless.BeetleTracker.Identity;
using Nameless.BeetleTracker.Models.WebApi.Account;
using Nameless.BeetleTracker.Resources;
using Nameless.BeetleTracker.Security;
using Nameless.BeetleTracker.WebApi;
using Nameless.BeetleTracker.WebApi.Results;

namespace Nameless.BeetleTracker.Controllers.WebApi {

    /// <summary>
    /// Account controller
    /// </summary>
    [Authorize]
    [ApiVersion("1")]
    public class AccountController : WebApiController {

        #region Private Constants

        private const string LOCAL_LOGIN_PROVIDER = "Local";

        #endregion Private Constants

        #region Private Fields

        private UserManager<IdentityUser> _userManager;
        private ISecureDataFormat<AuthenticationTicket> _accessTokenFormat;

        #endregion Private Fields

        #region Private Properties

        private IAuthenticationManager Authentication {
            get { return Request.GetOwinContext().Authentication; }
        }

        #endregion Private Properties

        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of <see cref="AccountController"/>
        /// </summary>
        /// <param name="userManager">The user manager</param>
        /// <param name="accessTokenFormat">The access token formatter.</param>
        public AccountController(IdentityUserManager userManager, ISecureDataFormat<AuthenticationTicket> accessTokenFormat) {
            Prevent.ParameterNull(userManager, nameof(userManager));
            Prevent.ParameterNull(accessTokenFormat, nameof(accessTokenFormat));

            _userManager = userManager;
            _accessTokenFormat = accessTokenFormat;
        }

        #endregion Public Constructors

        #region Public Methods

        /// <summary>
        /// Retrieves the user information.
        /// </summary>
        /// <returns>User information as response.</returns>
        [HttpGet]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("userinfo")]
        public IHttpActionResult GetUserInfo() {
            var externalLogin = ExternalLoginData.FromIdentity(User.Identity as ClaimsIdentity);

            return Ok(new UserInfoViewModel {
                Email = User.Identity.GetUserName(),
                HasRegistered = externalLogin == null,
                LoginProvider = externalLogin != null ? externalLogin.LoginProvider : null
            });
        }

        /// <summary>
        /// Log out from the application.
        /// </summary>
        /// <returns>Log out confirmation response.</returns>
        [HttpPost]
        [Route("logout")]
        public IHttpActionResult Logout() {
            Authentication.SignOut(CookieAuthenticationDefaults.AuthenticationType);

            return Ok();
        }

        /// <summary>
        /// Retrieves manage information.
        /// </summary>
        /// <param name="returnUrl">The return URL.</param>
        /// <param name="generateState">Whether will or not generate state.</param>
        /// <returns>The manage information as response.</returns>
        [HttpGet]
        [Route("manageinfo")]
        public async Task<IHttpActionResult> GetManageInfo(string returnUrl, bool generateState = false) {
            var user = await _userManager.FindByIdAsync(User.Identity.GetUserId());
            if (user == null) { return null; }

            var logins = new List<UserLoginInfoViewModel>();
            var currentLogins = _userManager.GetLogins(user.Id);
            foreach (var linkedAccount in currentLogins) {
                logins.Add(new UserLoginInfoViewModel {
                    LoginProvider = linkedAccount.LoginProvider,
                    ProviderKey = linkedAccount.ProviderKey
                });
            }

            if (user.PasswordHash != null) {
                logins.Add(new UserLoginInfoViewModel {
                    LoginProvider = LOCAL_LOGIN_PROVIDER,
                    ProviderKey = user.UserName,
                });
            }
            return Ok(new ManageInfoViewModel {
                LocalLoginProvider = LOCAL_LOGIN_PROVIDER,
                Email = user.UserName,
                Logins = logins,
                ExternalLoginProviders = GetExternalLogins(returnUrl, generateState)
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("changepassword")]
        public async Task<IHttpActionResult> ChangePassword(ChangePasswordBindingModel model) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            var result = await _userManager.ChangePasswordAsync(User.Identity.GetUserId()
                , model.OldPassword
                , model.NewPassword);

            if (!result.Succeeded) {
                return GetErrorResult(result);
            }

            return Ok();
        }

        [HttpPost /* POST api/Account/SetPassword */]
        [Route("setpassword")]
        public async Task<IHttpActionResult> SetPassword(SetPasswordBindingModel model) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            var result = await _userManager.AddPasswordAsync(User.Identity.GetUserId()
                , model.NewPassword);

            if (!result.Succeeded) {
                return GetErrorResult(result);
            }

            return Ok();
        }

        [HttpPost /* POST api/Account/AddExternalLogin */]
        [Route("addexternallogin")]
        public async Task<IHttpActionResult> AddExternalLogin(AddExternalLoginBindingModel model) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);

            var ticket = _accessTokenFormat.Unprotect(model.ExternalAccessToken);
            if (ticket == null || ticket.Identity == null || (ticket.Properties != null && ticket.Properties.ExpiresUtc.HasValue && ticket.Properties.ExpiresUtc.Value < DateTimeOffset.UtcNow)) {
                return BadRequest(T["External login failure."].Value);
            }

            var externalData = ExternalLoginData.FromIdentity(ticket.Identity);
            if (externalData == null) {
                return BadRequest(T["The external login is already associated with an account."].Value);
            }

            var result = await _userManager.AddLoginAsync(User.Identity.GetUserId()
                , new UserLoginInfo(externalData.LoginProvider
                    , externalData.ProviderKey));
            if (!result.Succeeded) {
                return GetErrorResult(result);
            }

            return Ok();
        }

        [HttpPost /* POST api/Account/RemoveLogin */]
        [Route("removelogin")]
        public async Task<IHttpActionResult> RemoveLogin(RemoveLoginBindingModel model) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            IdentityResult result;

            if (model.LoginProvider == LOCAL_LOGIN_PROVIDER) {
                result = await _userManager.RemovePasswordAsync(User.Identity.GetUserId());
            } else {
                result = await _userManager.RemoveLoginAsync(User.Identity.GetUserId()
                    , new UserLoginInfo(model.LoginProvider
                        , model.ProviderKey));
            }

            if (!result.Succeeded) {
                return GetErrorResult(result);
            }

            return Ok();
        }

        [HttpGet /* GET api/Account/ExternalLogin */]
        [OverrideAuthentication]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalCookie)]
        [AllowAnonymous]
        [Route("externallogin", Name = "ExternalLogin")]
        public async Task<IHttpActionResult> GetExternalLogin(string provider, string error = null) {
            if (error != null) {
                return Redirect(string.Concat(Url.Content("~/"), "#error=", Uri.EscapeDataString(error)));
            }

            if (!User.Identity.IsAuthenticated) {
                return new ChallengeResult(provider, this);
            }

            var externalLogin = ExternalLoginData.FromIdentity(User.Identity as ClaimsIdentity);
            if (externalLogin == null) {
                return InternalServerError();
            }

            if (externalLogin.LoginProvider != provider) {
                Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);
                return new ChallengeResult(provider, this);
            }

            var user = await _userManager.FindAsync(new UserLoginInfo(externalLogin.LoginProvider
                , externalLogin.ProviderKey));
            var hasRegistered = user != null;
            if (hasRegistered) {
                Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);

                var oAuthIdentity = await user.GenerateUserIdentityAsync(_userManager
                    , OAuthDefaults.AuthenticationType);
                var cookieIdentity = await user.GenerateUserIdentityAsync(_userManager
                    , CookieAuthenticationDefaults.AuthenticationType);

                var properties = ApplicationOAuthProvider.CreateProperties(user.UserName);
                Authentication.SignIn(properties, oAuthIdentity, cookieIdentity);
            } else {
                var claims = externalLogin.GetClaims();
                var identity = new ClaimsIdentity(claims, OAuthDefaults.AuthenticationType);
                Authentication.SignIn(identity);
            }

            return Ok();
        }

        [HttpGet /* GET api/Account/ExternalLogins?returnUrl=%2F&generateState=true */]
        [AllowAnonymous]
        [Route("externallogins")]
        public IEnumerable<ExternalLoginViewModel> GetExternalLogins(string returnUrl, bool generateState = false) {
            var descriptions = Authentication.GetExternalAuthenticationTypes();
            var logins = new List<ExternalLoginViewModel>();

            string state = null;

            if (generateState) {
                const int strengthInBits = 256;
                state = RandomOAuthStateGenerator.Generate(strengthInBits);
            }

            foreach (var description in descriptions) {
                var login = new ExternalLoginViewModel {
                    Name = description.Caption,
                    Url = Url.Route("ExternalLogin", new {
                        provider = description.AuthenticationType,
                        response_type = "token",
                        client_id = SecurityData.PublicClientID,
                        redirect_uri = new Uri(Request.RequestUri, returnUrl).AbsoluteUri,
                        state = state
                    }),
                    State = state
                };
                logins.Add(login);
            }

            return logins;
        }

        [HttpPost /* POST api/Account/Register */]
        [AllowAnonymous]
        [Route("register")]
        public async Task<IHttpActionResult> Register(RegisterBindingModel model) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            var user = new IdentityUser {
                UserName = model.Email, // We will use the e-mail as user name.
                Email = model.Email,
                FullName = model.FullName,
                ProfilePicture = Resource.no_user_image.ToByteArray() // "Empty" image as the default profile picture.
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded) {
                return GetErrorResult(result);
            }

            return Ok();
        }

        [HttpPost /* POST api/Account/RegisterExternal */]
        [OverrideAuthentication]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("registerexternal")]
        public async Task<IHttpActionResult> RegisterExternal(RegisterExternalBindingModel model) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            var info = await Authentication.GetExternalLoginInfoAsync();
            if (info == null) {
                return InternalServerError();
            }

            var user = new IdentityUser {
                UserName = model.Email,
                Email = model.Email
            };

            var result = await _userManager.CreateAsync(user);
            if (!result.Succeeded) {
                return GetErrorResult(result);
            }

            result = await _userManager.AddLoginAsync(user.Id, info.Login);
            if (!result.Succeeded) {
                return GetErrorResult(result);
            }

            return Ok();
        }

        #endregion Public Methods

        #region Protected Override Methods

        protected override void Dispose(bool disposing) {
            if (disposing && _userManager != null) {
                _userManager.Dispose();
                _userManager = null;
            }

            base.Dispose(disposing);
        }

        #endregion Protected Override Methods

        #region Private Methods

        private IHttpActionResult GetErrorResult(IdentityResult result) {
            if (result == null) {
                return InternalServerError();
            }

            if (!result.Succeeded) {
                if (result.Errors != null) {
                    foreach (var error in result.Errors) {
                        ModelState.AddModelError(string.Empty, error);
                    }
                }

                if (ModelState.IsValid) {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }

        #endregion Private Methods

        #region Private Inner Class

        private class ExternalLoginData {

            #region Public Properties

            public string LoginProvider { get; set; }
            public string ProviderKey { get; set; }
            public string UserName { get; set; }

            #endregion Public Properties

            #region Public Methods

            public IList<Claim> GetClaims() {
                var claims = new List<Claim> {
                    new Claim(ClaimTypes.NameIdentifier, ProviderKey, null, LoginProvider)
                };

                if (UserName != null) {
                    claims.Add(new Claim(ClaimTypes.Name, UserName, null, LoginProvider));
                }

                return claims;
            }

            #endregion Public Methods

            #region Public Static Methods

            public static ExternalLoginData FromIdentity(ClaimsIdentity identity) {
                if (identity == null) {
                    return null;
                }

                var providerKeyClaim = identity.FindFirst(ClaimTypes.NameIdentifier);

                if (providerKeyClaim == null || string.IsNullOrEmpty(providerKeyClaim.Issuer) || string.IsNullOrEmpty(providerKeyClaim.Value)) {
                    return null;
                }

                if (providerKeyClaim.Issuer == ClaimsIdentity.DefaultIssuer) {
                    return null;
                }

                return new ExternalLoginData {
                    LoginProvider = providerKeyClaim.Issuer,
                    ProviderKey = providerKeyClaim.Value,
                    UserName = identity.FindFirstValue(ClaimTypes.Name)
                };
            }

            #endregion Public Static Methods
        }

        #endregion Private Inner Class

        #region Private Static Inner Class

        private static class RandomOAuthStateGenerator {

            #region Private Static Read-Only Fields

            private static readonly RandomNumberGenerator Random = new RNGCryptoServiceProvider();

            #endregion Private Static Read-Only Fields

            #region Public Static Methods

            public static string Generate(int strengthInBits) {
                const int bitsPerByte = 8;

                if (strengthInBits % bitsPerByte != 0) {
                    throw new ArgumentException("strengthInBits must be evenly divisible by 8.", "strengthInBits");
                }

                var strengthInBytes = strengthInBits / bitsPerByte;
                var data = new byte[strengthInBytes];

                Random.GetBytes(data);

                return HttpServerUtility.UrlTokenEncode(data);
            }

            #endregion Public Static Methods
        }

        #endregion Private Static Inner Class
    }
}