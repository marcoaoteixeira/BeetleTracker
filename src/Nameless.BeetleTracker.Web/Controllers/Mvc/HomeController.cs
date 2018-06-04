using System.Web.Mvc;
using Nameless.BeetleTracker.Mvc;

namespace Nameless.BeetleTracker.Controllers.Mvc {

    public class HomeController : MvcController {

        #region Public Methods

        [HttpGet]
        public ActionResult Index() {
            return View();
        }

        #endregion Public Methods
    }
}