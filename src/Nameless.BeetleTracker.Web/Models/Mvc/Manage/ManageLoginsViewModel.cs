using System.Collections.Generic;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

namespace Nameless.BeetleTracker.Models.Mvc.Manage {
    public class ManageLoginsViewModel {
        #region Public Properties

        public IList<UserLoginInfo> CurrentLogins { get; set; } = new List<UserLoginInfo>();
        public IList<AuthenticationDescription> OtherLogins { get; set; } = new List<AuthenticationDescription>();

        #endregion
    }
}