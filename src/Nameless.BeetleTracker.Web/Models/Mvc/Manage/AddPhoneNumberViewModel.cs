using System.ComponentModel.DataAnnotations;
using Nameless.BeetleTracker.Resources;

namespace Nameless.BeetleTracker.Models.Mvc.Manage {
    public class AddPhoneNumberViewModel {
        #region Public Properties

        [Required(ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "Required")]
        [Phone(ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "Phone")]
        [Display(ResourceType = typeof(Displays), Name = "PhoneNumber")]
        public string Number { get; set; }

        #endregion
    }
}