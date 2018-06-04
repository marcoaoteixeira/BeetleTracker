using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Nameless.BeetleTracker.WebApi.Filters {

    /// <summary>
    /// Action filter for check if model contains <c>null</c> values.
    /// </summary>
    public class CheckModelForNullAttribute : ActionFilterAttribute {

        #region Private Read-Only Fields

        private readonly Func<Dictionary<string, object>, bool> _validate;

        #endregion Private Read-Only Fields

        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of <see cref="CheckModelForNullAttribute"/>.
        /// </summary>
        public CheckModelForNullAttribute()
            : this(argument => !argument.ContainsValue(null)) { }

        /// <summary>
        /// Initializes a new instance of <see cref="CheckModelForNullAttribute"/>.
        /// </summary>
        /// <param name="validate">The function to check <c>null</c> values.</param>
        public CheckModelForNullAttribute(Func<Dictionary<string, object>, bool> validate) {
            Prevent.ParameterNull(validate, nameof(validate));

            _validate = validate;
        }

        #endregion Public Constructors

        #region Public Override Methods

        /// <inheritdoc/>
        public override void OnActionExecuting(HttpActionContext actionContext) {
            if (!_validate(actionContext.ActionArguments)) {
                actionContext.Response = actionContext.Request.CreateErrorResponse(HttpStatusCode.BadRequest
                    , Properties.Resources.CheckModelForNullAttributeErrorMessage);
            }
        }

        #endregion Public Override Methods
    }
}