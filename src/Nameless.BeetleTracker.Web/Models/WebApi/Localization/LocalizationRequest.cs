using System.ComponentModel.DataAnnotations;

namespace Nameless.BeetleTracker.Models.WebApi.Localization {

    public class LocalizationRequest {

        #region Public Properties

        [Required]
        public string Source { get; set; }
        [Required]
        public string Value { get; set; }
        public object[] Arguments { get; set; }
        public string Culture { get; set; }

        #endregion Public Properties
    }
}