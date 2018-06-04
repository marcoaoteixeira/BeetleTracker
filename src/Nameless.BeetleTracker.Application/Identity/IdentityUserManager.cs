using System;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Nameless.BeetleTracker.Data;
using Nameless.BeetleTracker.Identity.Services;
using Nameless.BeetleTracker.Identity.Stores;

namespace Nameless.BeetleTracker.Identity {

    /// <summary>
    /// Default implementation of <see cref="UserManager{IdentityUser}"/>
    /// </summary>
    public class IdentityUserManager : UserManager<IdentityUser> {

        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of <see cref="IdentityUserManager"/>
        /// </summary>
        /// <param name="store">An instance of <see cref="IUserStore{TUser}"/>.</param>
        public IdentityUserManager(IUserStore<IdentityUser> store)
            : base(store) { }

        #endregion Public Constructors

        #region Public Static Methods

        /// <summary>
        /// Creates an instance of <see cref="IdentityUserManager"/>.
        /// </summary>
        /// <param name="options">The identity factory options.</param>
        /// <param name="context">The OWIN context.</param>
        /// <returns>An instance of <see cref="IdentityUserManager"/></returns>
        public static IdentityUserManager Create(IdentityFactoryOptions<IdentityUserManager> options, IOwinContext context) {
            var manager = new IdentityUserManager(new UserStore<IdentityUser>(context.Get<IDatabase>()));
            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<IdentityUser>(manager) {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };
            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator {
                RequiredLength = 6,
                RequireNonLetterOrDigit = true,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true,
            };

            // Configure user lockout defaults
            manager.UserLockoutEnabledByDefault = true;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            manager.MaxFailedAccessAttemptsBeforeLockout = 5;

            // Register two factor authentication providers. This application uses Phone and Emails
            // as a step of receiving a code for verifying the user You can write your own provider
            // and plug it in here.
            manager.RegisterTwoFactorProvider("Phone Code", new PhoneNumberTokenProvider<IdentityUser> {
                MessageFormat = Properties.Resources.PhoneNumberTokenProviderMessageFormat
            });
            manager.RegisterTwoFactorProvider("Email Code", new EmailTokenProvider<IdentityUser> {
                Subject = Properties.Resources.EmailTokenProviderSubject,
                BodyFormat = Properties.Resources.EmailTokenProviderMessageFormat
            });
            manager.EmailService = context.Get<IEmailService>();
            manager.SmsService = context.Get<ISmsService>();

            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null) {
                manager.UserTokenProvider = new DataProtectorTokenProvider<IdentityUser>(dataProtectionProvider.Create("ASP.NET Identity"));
            }

            return manager;
        }

        #endregion Public Static Methods
    }
}