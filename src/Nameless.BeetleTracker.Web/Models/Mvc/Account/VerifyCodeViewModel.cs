using System.ComponentModel.DataAnnotations;
using Nameless.BeetleTracker.Resources;

namespace Nameless.BeetleTracker.Models.Mvc.Account {

    /// <summary>
    /// Verify code (two factor authentication) view model.
    /// </summary>
    public class VerifyCodeViewModel {

        #region Public Properties

        /// <summary>
        /// Gets or sets the provider name.
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "Required")]
        public string Provider { get; set; }

        /// <summary>
        /// Gets or sets the code.
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "Required")]
        [Display(ResourceType = typeof(Displays), Name = "Code")]
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets the return URL.
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        /// Whether will or not remember the browser.
        /// </summary>
        [Display(ResourceType = typeof(Displays), Name = "RememberThisBrowser")]
        public bool RememberBrowser { get; set; }

        /// <summary>
        /// Gets or sets the "remember me" flag.
        /// </summary>
        public bool RememberMe { get; set; }

        #endregion Public Properties
    }
}