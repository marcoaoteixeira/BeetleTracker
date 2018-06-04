using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace Nameless.BeetleTracker {

    public partial class StartUp {

        #region Private Methods

        private void ConfigureMvcRouting(RouteCollection routes) {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }

        private void ConfigureWebApiRouting(HttpRouteCollection routes) {
            routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/v{version:apiVersion}/{controller}",
                defaults: new { apiVersion = 1 }
            );
        }

        #endregion Private Methods
    }
}