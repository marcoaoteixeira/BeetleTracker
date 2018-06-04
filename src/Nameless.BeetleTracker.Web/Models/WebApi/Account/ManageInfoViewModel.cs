using System.Collections.Generic;
using System.Linq;

namespace Nameless.BeetleTracker.Models.WebApi.Account {
    public class ManageInfoViewModel {
        #region Public Properties

        public string LocalLoginProvider { get; set; }
        public string Email { get; set; }
        public IEnumerable<UserLoginInfoViewModel> Logins { get; set; } = Enumerable.Empty<UserLoginInfoViewModel>();
        public IEnumerable<ExternalLoginViewModel> ExternalLoginProviders { get; set; } = Enumerable.Empty<ExternalLoginViewModel>();

        #endregion
    }
}