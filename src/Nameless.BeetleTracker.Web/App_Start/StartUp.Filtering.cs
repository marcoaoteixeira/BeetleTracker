using System.Web.Http;
using System.Web.Http.Filters;
using System.Web.Mvc;
using Microsoft.Owin.Security.OAuth;

namespace Nameless.BeetleTracker {

    public partial class StartUp {

        #region Private Methods

        private void ConfigureMvcFiltering(GlobalFilterCollection filters) {
            filters.Add(new HandleErrorAttribute());
        }

        private void ConfigureWebApiFiltering(HttpFilterCollection filters) {
            filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));
        }

        #endregion Private Methods
    }
}