using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Owin;
using Nameless.BeetleTracker.Settings;

namespace Nameless.BeetleTracker.Security {

    /// <summary>
    /// Injects the <see cref="IApplicationContext"/> inside the OWIN context.
    /// </summary>
    public class ApplicationContextMiddleware : OwinMiddleware {

        #region Public Static Read-Only Fields

        /// <summary>
        /// Gets the <see cref="IApplicationContext"/> OWIN key.
        /// </summary>
        public static readonly string ApplicationContextKey = "owin:ApplicationContext";

        #endregion Public Static Read-Only Fields

        #region Private Fields

        private ApplicationSettings _settings;

        #endregion

        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of <see cref="ApplicationContextMiddleware"/>.
        /// </summary>
        /// <param name="next">The next middleware.</param>
        /// <param name="settings">The application's settings.</param>
        public ApplicationContextMiddleware(OwinMiddleware next, ApplicationSettings settings)
            : base(next) {
            Prevent.ParameterNull(settings, nameof(settings));

            _settings = settings;
        }

        #endregion Public Constructors

        #region Public Override Methods

        /// <inheritdoc/>
        public override async Task Invoke(IOwinContext context) {
            var userInfo = new UserInfo(Guid.Empty, "Anonymous", "anonymous@anonymous.com", new string[0], new Dictionary<string, string>(), true);
            var applicationInfo = new ApplicationInfo(_settings.ApplicationID, _settings.ApplicationName);
            var applicationContext = new ApplicationContext(applicationInfo, userInfo);

            context.Set(ApplicationContextKey, applicationContext);

            await Next.Invoke(context);
        }

        #endregion Public Override Methods
    }
}