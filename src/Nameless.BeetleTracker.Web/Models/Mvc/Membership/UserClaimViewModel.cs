using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Nameless.BeetleTracker.Resources;
using Newtonsoft.Json;

namespace Nameless.BeetleTracker.Models.Mvc.Membership {
    public class UserClaimViewModel {

        #region Public Properties

        [JsonProperty(PropertyName = "id")]
        [HiddenInput]
        public long ID { get; set; }

        [JsonProperty(PropertyName = "type")]
        [Display(ResourceType = typeof(Displays), Name = "Type")]
        [Required(ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "Required")]
        [StringLength(256, ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "StringLength", MinimumLength = 3)]
        public string Type { get; set; }

        [JsonProperty(PropertyName = "value")]
        [Display(ResourceType = typeof(Displays), Name = "Value")]
        [Required(ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "Required")]
        [StringLength(256, ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "StringLength", MinimumLength = 3)]
        public string Value { get; set; }

        #endregion
    }
}