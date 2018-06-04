using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using Nameless.BeetleTracker.Identity;

namespace Nameless.BeetleTracker.Security {
    /// <summary>
    /// Default implementation of <see cref="OAuthAuthorizationServerProvider"/>.
    /// </summary>
    public class ApplicationOAuthProvider : OAuthAuthorizationServerProvider {

        #region Private Read-Only Fields

        private readonly string _publicClientId;

        #endregion

        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of <see cref="ApplicationOAuthProvider"/>.
        /// </summary>
        /// <param name="publicClientId">The public client ID.</param>
        public ApplicationOAuthProvider(string publicClientId) {
            Prevent.ParameterNull(publicClientId, nameof(publicClientId));

            _publicClientId = publicClientId;
        }

        #endregion

        #region Public Static Methods

        /// <summary>
        /// Creates an <see cref="AuthenticationProperties"/> for the user name.
        /// </summary>
        /// <param name="userName">The user name.</param>
        /// <returns>An instance of <see cref="AuthenticationProperties"/> with the user name property.</returns>
        public static AuthenticationProperties CreateProperties(string userName) {
            return new AuthenticationProperties(new Dictionary<string, string> {
                { "userName", userName }
            });
        }

        #endregion

        #region Public Override Methods

        /// <inheritdoc />
        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context) {
            var userManager = context.OwinContext.GetUserManager<IdentityUserManager>();
            var user = await userManager.FindAsync(context.UserName, context.Password);

            if (user == null) {
                context.SetError("invalid_grant", "The user name or password is incorrect.");
                return;
            }

            var oAuthIdentity = await user.GenerateUserIdentityAsync(userManager, OAuthDefaults.AuthenticationType);
            var cookiesIdentity = await user.GenerateUserIdentityAsync(userManager, CookieAuthenticationDefaults.AuthenticationType);
            var properties = CreateProperties(user.UserName);
            var ticket = new AuthenticationTicket(oAuthIdentity, properties);
            context.Validated(ticket);
            context.Request.Context.Authentication.SignIn(cookiesIdentity);
        }

        /// <inheritdoc />
        public override Task TokenEndpoint(OAuthTokenEndpointContext context) {
            foreach (var kvp in context.Properties.Dictionary) {
                context.AdditionalResponseParameters.Add(kvp.Key, kvp.Value);
            }

            return Task.FromResult<object>(null);
        }

        /// <inheritdoc />
        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context) {
            // Resource owner password credentials does not provide a client ID.
            if (context.ClientId == null) {
                context.Validated();
            }

            return Task.FromResult<object>(null);
        }

        /// <inheritdoc />
        public override Task ValidateClientRedirectUri(OAuthValidateClientRedirectUriContext context) {
            if (context.ClientId == _publicClientId) {
                var expectedRootUri = new Uri(context.Request.Uri, "/");

                if (expectedRootUri.AbsoluteUri == context.RedirectUri) {
                    context.Validated();
                }
            }

            return Task.FromResult<object>(null);
        }

        #endregion
    }
}