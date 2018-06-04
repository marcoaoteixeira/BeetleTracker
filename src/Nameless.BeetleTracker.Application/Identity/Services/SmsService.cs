using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace Nameless.BeetleTracker.Identity.Services {

    public class SmsService : ISmsService {

        #region ISmsService Members

        public Task SendAsync(IdentityMessage message) {
            return Task.CompletedTask;
        }

        #endregion ISmsService Members
    }
}