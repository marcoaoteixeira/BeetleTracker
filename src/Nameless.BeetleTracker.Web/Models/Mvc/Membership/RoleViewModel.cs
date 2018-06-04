using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Nameless.BeetleTracker.Resources;
using Newtonsoft.Json;

namespace Nameless.BeetleTracker.Models.Mvc.Membership {
    public class RoleViewModel {

        #region Public Properties

        [JsonProperty(PropertyName = "id")]
        [HiddenInput]
        public Guid ID { get; set; }

        [JsonProperty(PropertyName = "name")]
        [Display(ResourceType = typeof(Displays), Name = "Name")]
        [Required(ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "Required")]
        [StringLength(256, ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "StringLength", MinimumLength = 3)]
        public string Name { get; set; }

        #endregion
    }
}