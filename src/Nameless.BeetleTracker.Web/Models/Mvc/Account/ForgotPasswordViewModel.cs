using System.ComponentModel.DataAnnotations;
using Nameless.BeetleTracker.Resources;

namespace Nameless.BeetleTracker.Models.Mvc.Account {
    public class ForgotPasswordViewModel {
        #region Public Properties

        [Required(ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "Required")]
        [EmailAddress(ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "EmailAddress")]
        [Display(ResourceType = typeof(Displays), Name = "Email")]
        public string Email { get; set; }

        #endregion
    }
}