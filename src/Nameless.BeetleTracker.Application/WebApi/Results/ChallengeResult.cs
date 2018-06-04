using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace Nameless.BeetleTracker.WebApi.Results {

    /// <summary>
    /// Challenge result against credentials.
    /// </summary>
    public class ChallengeResult : IHttpActionResult {

        #region Public Properties

        /// <summary>
        /// Gets or sets the login provider.
        /// </summary>
        public string LoginProvider { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="HttpRequestMessage"/>.
        /// </summary>
        public HttpRequestMessage Request { get; set; }

        #endregion Public Properties

        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of <see cref="ChallengeResult"/>.
        /// </summary>
        /// <param name="loginProvider">The login provider.</param>
        /// <param name="controller">The API controller instance.</param>
        public ChallengeResult(string loginProvider, ApiController controller) {
            LoginProvider = loginProvider;
            Request = controller.Request;
        }

        #endregion Public Constructors

        #region IHttpActionResult Members

        /// <inheritdoc/>
        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken) {
            var wrapper = new HttpContextWrapper(HttpContext.Current);
            wrapper.GetOwinContext().Authentication.Challenge(LoginProvider);

            var response = new HttpResponseMessage(HttpStatusCode.Unauthorized) {
                RequestMessage = Request
            };

            return Task.FromResult(response);
        }

        #endregion IHttpActionResult Members
    }
}