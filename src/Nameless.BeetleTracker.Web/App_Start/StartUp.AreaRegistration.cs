using System.Web.Mvc;

namespace Nameless.BeetleTracker {

    public partial class StartUp {

        #region Private Methods

        private void ConfigureMvcAreaRegistration() {
            AreaRegistration.RegisterAllAreas();
        }

        #endregion Private Methods
    }
}