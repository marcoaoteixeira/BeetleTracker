using System.Web.Http;
using Nameless.BeetleTracker.Localization;
using Nameless.BeetleTracker.Logging;

namespace Nameless.BeetleTracker.WebApi {

    /// <summary>
    /// Abstract controller for ASP.Net Web API
    /// </summary>
    public abstract class WebApiController : ApiController {

        #region Private Fields

        private ILogger _logger;
        private IStringLocalizer _localizer;

        #endregion Private Fields

        #region Public Properties

        /// <summary>
        /// Gets or sets the Logger value.
        /// </summary>
        public ILogger Logger {
            get { return _logger ?? (_logger = NullLogger.Instance); }
            set { _logger = value ?? NullLogger.Instance; }
        }

        /// <summary>
        /// Gets or sets the StringLocalizer value.
        /// </summary>
        public IStringLocalizer T {
            get { return _localizer ?? (_localizer = NullStringLocalizer.Instance); }
            set { _localizer = value ?? NullStringLocalizer.Instance; }
        }

        #endregion Public Properties
    }
}