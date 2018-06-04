namespace Nameless.BeetleTracker.Models.WebApi.Account {
    public class UserInfoViewModel {
        #region Public Properties

        public string Email { get; set; }
        public bool HasRegistered { get; set; }
        public string LoginProvider { get; set; }

        #endregion
    }
}