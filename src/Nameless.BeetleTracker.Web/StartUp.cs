using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.Owin;
using Nameless.BeetleTracker;
using Owin;

[assembly: OwinStartup(typeof(StartUp))]

namespace Nameless.BeetleTracker {

    public partial class StartUp {

        #region Private Fields

        private HttpConfiguration _configuration;

        #endregion Private Fields

        #region Public Constructors

        public StartUp() {
            _configuration = GlobalConfiguration.Configuration;
        }

        #endregion Public Constructors

        #region Public Methods

        public void Configuration(IAppBuilder app) {
            ConfigureInfrastructure();

            ConfigureWebApiFiltering(_configuration.Filters);
            ConfigureWebApiInfrastructure(_configuration);
            ConfigureWebApiRouting(_configuration.Routes);

            ConfigureMvcAreaRegistration();
            ConfigureMvcFiltering(GlobalFilters.Filters);
            ConfigureMvcRouting(RouteTable.Routes);

            ConfigureAuth(app);
            ConfigureCompositionRoot(app, _configuration);

            _configuration.EnsureInitialized();
        }

        #endregion Public Methods
    }
}