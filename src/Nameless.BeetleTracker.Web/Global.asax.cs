using System;
using System.Web;

namespace Nameless.BeetleTracker {

    /// <summary>
    /// Global ASAX code
    /// </summary>
    public class Global : HttpApplication {

        #region Protected Methods

        /// <summary>
        /// Called when the first resource (such as a page) in an ASP.NET application is requested.
        /// The Application_Start method is called only one time during the life cycle of an
        /// application. You can use this method to perform startup tasks such as loading data into
        /// the cache and initializing static values.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event arguments.</param>
        protected void Application_Start(object sender, EventArgs e) {
        }

        /// <summary>
        /// Occurs just before ASP.NET sends HTTP headers to the client.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event arguments.</param>
        protected void Application_PreSendRequestHeaders(object sender, EventArgs e) {
            // Version information can be used by anattacker to target specific attack on that Version which is disclosed.
            // Whenever browsersendsHTTP torequest to theserverin response we get response header which contains information of[Server, X - AspNet - Version, X - AspNetMvc - Version, X - Powered - By].
            Response.Headers.Remove("Server");
            Response.Headers.Remove("X-AspNet-Version");
        }

        #endregion Protected Methods
    }
}