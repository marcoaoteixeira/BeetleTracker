using System.ComponentModel.DataAnnotations;
using Nameless.BeetleTracker.Resources;

namespace Nameless.BeetleTracker.Models.Mvc.Account {
    public class LoginViewModel {
        #region Public Properties

        [Required(ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "Required")]
        [Display(ResourceType = typeof(Displays), Name = "Email")]
        [EmailAddress(ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "EmailAddress")]
        public string Email { get; set; }
        [Required(ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "Required")]
        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(Displays), Name = "Password")]
        public string Password { get; set; }
        [Display(ResourceType = typeof(Displays), Name = "RememberMe")]
        public bool RememberMe { get; set; }

        #endregion
    }
}