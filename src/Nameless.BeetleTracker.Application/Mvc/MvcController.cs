using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Nameless.BeetleTracker.Localization;
using Nameless.BeetleTracker.Logging;

namespace Nameless.BeetleTracker.Mvc {

    /// <summary>
    /// Abstract controller for ASP.Net MVC.
    /// </summary>
    public abstract class MvcController : Controller {

        #region Private Fields

        private ILogger _logger;
        private IStringLocalizer _localizer;

        #endregion

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