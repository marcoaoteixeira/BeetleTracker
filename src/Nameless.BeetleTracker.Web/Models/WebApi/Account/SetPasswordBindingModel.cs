using System.ComponentModel.DataAnnotations;
using Nameless.BeetleTracker.Resources;

namespace Nameless.BeetleTracker.Models.WebApi.Account {
    public class SetPasswordBindingModel {
        #region Public Properties

        [Required(ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "Required")]
        [StringLength(128, ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "StringLength", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(Displays), Name = "NewPassword")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(Displays), Name = "ConfirmNewPassword")]
        [Compare("NewPassword", ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "CompareNewPassword")]
        public string ConfirmPassword { get; set; }

        #endregion
    }
}