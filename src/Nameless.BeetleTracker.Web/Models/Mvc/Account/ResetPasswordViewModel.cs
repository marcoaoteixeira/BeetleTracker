using System.ComponentModel.DataAnnotations;
using Nameless.BeetleTracker.Resources;

namespace Nameless.BeetleTracker.Models.Mvc.Account {
    public class ResetPasswordViewModel {
        #region Public Properties

        [Required(ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "Required")]
        [EmailAddress(ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "EmailAddress")]
        [Display(ResourceType = typeof(Displays), Name = "Email")]
        public string Email { get; set; }
        [Required(ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "Required")]
        [StringLength(128, ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "StringLength", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(Displays), Name = "Password")]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(Displays), Name = "Confirm password")]
        [Compare("Password", ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "ComparePassword")]
        public string ConfirmPassword { get; set; }
        public string Code { get; set; }

        #endregion
    }
}