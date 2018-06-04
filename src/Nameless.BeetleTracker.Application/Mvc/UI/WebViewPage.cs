using System;
using System.Configuration;
using System.Web.Configuration;
using System.Web.Mvc;
using Nameless.BeetleTracker.Localization;

namespace Nameless.BeetleTracker.Mvc.UI {

    /// <summary>
    /// Abstract implementation of <see cref="System.Web.Mvc.WebViewPage{TModel}"/> to use the
    /// localization system.
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    public abstract class WebViewPage<TModel> : System.Web.Mvc.WebViewPage<TModel> {

        #region Public Properties

        private IStringLocalizer _localizer;
        /// <summary>
        /// Gets or sets the string localizer.
        /// </summary>
        public IStringLocalizer T {
            get { return _localizer ?? (_localizer = NullStringLocalizer.Instance); }
            set { _localizer = value ?? NullStringLocalizer.Instance; }
        }

        /// <summary>
        /// Gets the current controller name
        /// </summary>
        public string CurrentControllerName => (string)ViewContext.RouteData.Values["controller"];

        /// <summary>
        /// Gets the current action name
        /// </summary>
        public string CurrentActionName => (string)ViewContext.RouteData.Values["action"];

        private bool? _isDevelopmentEnvironment;
        /// <summary>
        /// Whether if the current environment is development or not.
        /// </summary>
        public bool IsDevelopmentEnvironment {
            get {
                if (_isDevelopmentEnvironment == null) {
                    var compilationSection = (CompilationSection)ConfigurationManager.GetSection(@"system.web/compilation");
                    _isDevelopmentEnvironment = compilationSection.Debug;
                }
                return _isDevelopmentEnvironment.Value;
            }
        }

        /// <summary>
        /// Gets or sets the application context.
        /// </summary>
        public IApplicationContext CurrentApplicationContext { get; set; }

        /// <summary>
        /// Gets the application name.
        /// </summary>
        public string ApplicationName {
            get {
                return (CurrentApplicationContext != null && CurrentApplicationContext.Application != null)
                  ? CurrentApplicationContext.Application.TenantName
                  : "ApplicationName";
            }
        }

        public int Random {
            get { return new System.Random().Next(); }
        }

        #endregion Public Properties
    }

    /// <summary>
    /// Dynamic model implementation of <see cref="WebViewPage{TModel}"/>.
    /// </summary>
    public abstract class WebViewPage : WebViewPage<dynamic> {
    }
}