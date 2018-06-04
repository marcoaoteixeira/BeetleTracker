namespace Nameless.BeetleTracker.Email {

    /// <summary>
    /// Represents the e-mail service.
    /// </summary>
    public interface IEmailService {

        #region Methods

        /// <summary>
        /// Sends the e-mail message.
        /// </summary>
        /// <param name="message">The e-mail message.</param>
        void Send(Message message);

        #endregion Methods
    }
}