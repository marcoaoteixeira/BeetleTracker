using System.ComponentModel.DataAnnotations;
using Nameless.BeetleTracker.Resources;

namespace Nameless.BeetleTracker.Models.WebApi.Account {
    public class AddExternalLoginBindingModel {
        #region Public Properties

        [Required(ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "Required")]
        [Display(ResourceType = typeof(Displays), Name = "ExternalAccessToken")]
        public string ExternalAccessToken { get; set; }

        #endregion
    }
}