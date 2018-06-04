using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Web.Mvc;

namespace Nameless.BeetleTracker.Models.Mvc.Manage {
    public class ConfigureTwoFactorViewModel {
        #region Public Properties

        public string SelectedProvider { get; set; }
        public ICollection<SelectListItem> Providers { get; set; } = new Collection<SelectListItem>();

        #endregion
    }
}