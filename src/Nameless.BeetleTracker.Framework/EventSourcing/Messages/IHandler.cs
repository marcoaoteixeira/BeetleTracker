namespace Nameless.BeetleTracker.EventSourcing.Messaging {

    /// <summary>
    /// Interface for handle messages.
    /// </summary>
    /// <typeparam name="TMessage">Type of the message.</typeparam>
    public interface IHandler<in TMessage> where TMessage : IMessage {

        #region Methods

        /// <summary>
        /// Handles a specific message.
        /// </summary>
        /// <param name="message">The message.</param>
        void Handle(TMessage message);

        #endregion Methods
    }
}