using System;

namespace Nameless.BeetleTracker.PubSub {

    /// <summary>
    /// Subscription interface for the publisher/subscriber system.
    /// </summary>
    /// <typeparam name="TMessage"></typeparam>
    public interface ISubscription<TMessage> {

        #region Methods

        /// <summary>
        /// Creates the message handler.
        /// </summary>
        /// <returns>The message handler.</returns>
        Action<TMessage> CreateHandler();

        #endregion Methods
    }
}