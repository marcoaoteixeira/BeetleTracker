using System.Globalization;
using System.Web.Http;
using Microsoft.Web.Http;
using Nameless.BeetleTracker.Environment;
using Nameless.BeetleTracker.Localization;
using Nameless.BeetleTracker.Models.WebApi.Localization;
using Nameless.BeetleTracker.WebApi;

namespace Nameless.BeetleTracker.Controllers.WebApi.v1 {

    /// <summary>
    /// Localization controller
    /// </summary>
    [ApiVersion("1")]
    public sealed class LocalizationController : WebApiController {

        #region Private Read-Only Fields

        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IStringLocalizerFactory _factory;

        #endregion Private Read-Only Fields

        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of <see cref="LocalizationController"/>.
        /// </summary>
        /// <param name="hostingEnvironment">The hosting environment.</param>
        /// <param name="factory">The string localization factory.</param>
        public LocalizationController(IHostingEnvironment hostingEnvironment, IStringLocalizerFactory factory) {
            Prevent.ParameterNull(hostingEnvironment, nameof(hostingEnvironment));
            Prevent.ParameterNull(factory, nameof(factory));

            _hostingEnvironment = hostingEnvironment;
            _factory = factory;
        }

        #endregion Public Constructors

        #region Public Methods

        /// <summary>
        /// Retrieves the localization for the specific string value.
        /// </summary>
        /// <param name="request">The localization request.</param>
        /// <returns>A JSON representation of the string.</returns>
        [HttpPost]
        public IHttpActionResult Get(LocalizationRequest request) {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            var culture = CultureInfo.GetCultureInfo(request.Culture ?? CultureInfo.CurrentUICulture.Name);
            var localizer = _factory.Create(null, request.Source, culture);
            var localization = localizer[request.Value, request.Arguments];

            return Json(new LocalizationResponse {
                Value = localization.Value,
                ResourceNotFound = localization.ResourceNotFound
            });
        }

        #endregion Public Methods
    }
}