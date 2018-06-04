using Nameless.BeetleTracker.Settings;

namespace Nameless.BeetleTracker.Data {
    /// <summary>
    /// Database settings.
    /// </summary>
    public class DatabaseSettings : SettingsBase {

        #region Public Properties

        /// <summary>
        /// Gets or sets the database provider name.
        /// </summary>
        public string ProviderName { get; set; } = string.Empty;
        /// <summary>
        /// Gets or sets the database connection string.
        /// </summary>
        public string ConnectionString { get; set; } = string.Empty;

        #endregion Public Properties
    }
}