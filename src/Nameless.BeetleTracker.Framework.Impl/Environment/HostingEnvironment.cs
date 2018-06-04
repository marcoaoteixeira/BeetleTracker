using System;
using System.Web;

namespace Nameless.BeetleTracker.Environment {

    /// <summary>
    /// Default implementation of <see cref="IHostingEnvironment"/>
    /// </summary>
    public class HostingEnvironment : IHostingEnvironment {

        #region IHostingEnvironment Members

        /// <inheritdoc/>
        public string ApplicationBasePath { get; } = AppDomain.CurrentDomain.BaseDirectory;

        /// <inheritdoc/>
        public object GetData(string key) {
            return AppDomain.CurrentDomain.GetData(key);
        }

        /// <inheritdoc/>
        public void SetData(string key, object data) {
            AppDomain.CurrentDomain.SetData(key, data);
        }

        #endregion IHostingEnvironment Members
    }
}