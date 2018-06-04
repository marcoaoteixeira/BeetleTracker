using System.Web.Mvc;
using Nameless.BeetleTracker.Mvc;

namespace Nameless.BeetleTracker.Controllers.Mvc {

    /// <summary>
    /// Controller that handle errors
    /// </summary>
    public class ErrorController : MvcController {

        #region Public Methods

        /// <summary>
        /// Redirects to the 404 page.
        /// </summary>
        /// <returns>An <see cref="ActionResult"/> instance.</returns>
        public ActionResult NotFound() {
            return View("404");
        }

        /// <summary>
        /// Redirects to the 500 page.
        /// </summary>
        /// <returns>An <see cref="ActionResult"/> instance.</returns>
        public ActionResult InternalServerError() {
            return View("500");
        }

        #endregion Public Methods
    }
}