using System.ComponentModel.DataAnnotations;
using Nameless.BeetleTracker.Resources;
using Newtonsoft.Json;

namespace Nameless.BeetleTracker.Models.Mvc.Membership {
    public class UserLoginViewModel {

        #region Public Properties

        [JsonProperty(PropertyName = "loginProvider")]
        [Display(ResourceType = typeof(Displays), Name = "LoginProvider")]
        [Required(ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "Required")]
        [StringLength(256, ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "StringLength", MinimumLength = 3)]
        public string LoginProvider { get; set; }

        [JsonProperty(PropertyName = "providerType")]
        [Display(ResourceType = typeof(Displays), Name = "ProviderType")]
        [Required(ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "Required")]
        [StringLength(256, ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "StringLength", MinimumLength = 3)]
        public string ProviderType { get; set; }

        #endregion
    }
}