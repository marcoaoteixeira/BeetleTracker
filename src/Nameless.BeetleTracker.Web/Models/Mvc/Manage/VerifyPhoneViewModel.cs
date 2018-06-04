using System.ComponentModel.DataAnnotations;
using Nameless.BeetleTracker.Resources;

namespace Nameless.BeetleTracker.Models.Mvc.Manage {

    /// <summary>
    /// Verify phone number view model.
    /// </summary>
    public class VerifyPhoneNumberViewModel {

        #region Public Properties

        /// <summary>
        /// Gets or sets the code.
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "Required")]
        [Display(ResourceType = typeof(Displays), Name = "Code")]
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets the phone number to verify.
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "Required")]
        [Phone(ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "Phone")]
        [Display(ResourceType = typeof(Displays), Name = "PhoneNumber")]
        public string PhoneNumber { get; set; }

        #endregion Public Properties
    }
}