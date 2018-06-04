using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Nameless.BeetleTracker.WebApi.Filters {

    /// <summary>
    /// Validate model state action filter.
    /// </summary>
    public sealed class ValidateModelStateAttribute : ActionFilterAttribute {

        #region Public Override Methods

        /// <inheritdoc />
        public override void OnActionExecuting(HttpActionContext actionContext) {
            base.OnActionExecuting(actionContext);

            if (actionContext.ControllerContext.Request.Method != HttpMethod.Post &&
                actionContext.ControllerContext.Request.Method != HttpMethod.Put) {
                return;
            }

            if (!actionContext.ModelState.IsValid) {
                actionContext.Response = actionContext.Request.CreateErrorResponse(
                    statusCode: HttpStatusCode.BadRequest,
                    modelState: actionContext.ModelState);
            }
        }

        #endregion Public Override Methods
    }
}