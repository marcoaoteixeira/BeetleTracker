using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace Nameless.BeetleTracker.Identity.Services {

    public class EmailService : IEmailService {

        #region IEmailService Members

        public Task SendAsync(IdentityMessage message) {
            return Task.CompletedTask;
        }

        #endregion IEmailService Members
    }
}