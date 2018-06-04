using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nameless.BeetleTracker.Settings;

namespace Nameless.BeetleTracker {

    /// <summary>
    /// Application's settings
    /// </summary>
    public sealed class ApplicationSettings : SettingsBase {

        #region Public Properties

        /// <summary>
        /// Gets or sets the application's ID.
        /// </summary>
        public Guid ApplicationID { get; set; }

        /// <summary>
        /// Gets or sets the application's name.
        /// </summary>
        public string ApplicationName { get; set; }

        #endregion Public Properties
    }
}