using Nameless.BeetleTracker.Settings;

namespace Nameless.BeetleTracker.Localization.Json {

    /// <summary>
    /// JSON localization options.
    /// </summary>
    public class LocalizationOptions : SettingsBase {

        #region Public Properties
        
        /// <summary>
        /// Gets or sets the resources path location.
        /// </summary>
        public string ResourcesRelativePath { get; set; }

        #endregion Public Properties
    }
}