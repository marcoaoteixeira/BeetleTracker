using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Web.Mvc;

namespace Nameless.BeetleTracker.Models.Mvc.Account {
    public class SendCodeViewModel {
        #region Public Properties

        public string SelectedProvider { get; set; }
        public ICollection<SelectListItem> Providers { get; set; } = new Collection<SelectListItem>();
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }

        #endregion
    }
}