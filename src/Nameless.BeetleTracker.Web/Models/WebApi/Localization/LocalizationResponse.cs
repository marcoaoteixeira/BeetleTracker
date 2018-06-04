using Newtonsoft.Json;

namespace Nameless.BeetleTracker.Models.WebApi.Localization {

    /// <summary>
    /// Localization object response.
    /// </summary>
    public class LocalizationResponse {

        #region Public Properties

        /// <summary>
        /// Gets or sets the localization value.
        /// </summary>
        [JsonProperty("value")]
        public string Value { get; set; }

        /// <summary>
        /// Whether if resource was found or not.
        /// </summary>
        [JsonProperty("resourceNotFound")]
        public bool ResourceNotFound { get; set; }

        #endregion Public Properties
    }
}