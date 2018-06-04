using System.Diagnostics;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Nameless.BeetleTracker.WebApi.Filters {
    /// <summary>
    /// Implementation of <see cref="ActionFilterAttribute"/> to use <see cref="Stopwatch"/>.
    /// </summary>
    public class StopwatchActionFilter : ActionFilterAttribute {

        #region Private Read-Only Fields

        private readonly Stopwatch _stopwatch;

        #endregion

        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of <see cref="StopwatchActionFilter"/>.
        /// </summary>
        public StopwatchActionFilter() {
            _stopwatch = Stopwatch.StartNew();
        }

        #endregion

        #region Public Override Methods

        /// <inheritdoc />
        public override void OnActionExecuting(HttpActionContext actionContext) {
            base.OnActionExecuting(actionContext);

            _stopwatch.Start();
        }

        /// <inheritdoc />
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext) {
            base.OnActionExecuted(actionExecutedContext);

            _stopwatch.Stop();

            var response = actionExecutedContext.Response;

            response.Headers.Add("X-Stopwatch", string.Format("{0} ms", _stopwatch.ElapsedMilliseconds));
        }

        #endregion
    }
}
