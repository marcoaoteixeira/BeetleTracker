using System.Web.Http;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using WebApiDefaultInlineConstraintResolver = System.Web.Http.Routing.DefaultInlineConstraintResolver;
using WebApiVersionRouteConstraint = Microsoft.Web.Http.Routing.ApiVersionRouteConstraint;

namespace Nameless.BeetleTracker {

    public partial class StartUp {

        #region Private Methods

        private void ConfigureInfrastructure() {
            JsonConvert.DefaultSettings = () => {
                return new JsonSerializerSettings {
                    ContractResolver = new DefaultContractResolver {
                        IgnoreSerializableAttribute = true
                    }
                };
            };
        }

        private void ConfigureMvcInfrastructure() {
            // Version information can be used by anattacker to target specific attack on that Version which is disclosed.
            // Whenever browsersendsHTTP torequest to theserverin response we get response header which contains information of[Server, X - AspNet - Version, X - AspNetMvc - Version, X - Powered - By].
            MvcHandler.DisableMvcResponseHeader = true;
        }

        private void ConfigureWebApiInfrastructure(HttpConfiguration configuration) {
            // Web API configuration and services Configure Web API to use only bearer token authentication.
            configuration.SuppressDefaultHostAuthentication();

            var routeConstraintResolver = new WebApiDefaultInlineConstraintResolver {
                ConstraintMap = {
                    ["apiVersion"] = typeof(WebApiVersionRouteConstraint)
                }
            };
            configuration.MapHttpAttributeRoutes(routeConstraintResolver);
            configuration.AddApiVersioning(_ => {
                _.AssumeDefaultVersionWhenUnspecified = true;
                _.ReportApiVersions = true;
            });
        }

        #endregion Private Methods
    }
}