namespace Nameless.BeetleTracker.EventSourcing.Events {

    /// <summary>
    /// Interface that defines methods for events publishing.
    /// </summary>
    public interface IEventPublisher {

        #region Methods

        /// <summary>
        /// Publishes an event.
        /// </summary>
        /// <typeparam name="TEvent">Type of the event.</typeparam>
        /// <param name="evt">The event.</param>
        void Publish<TEvent>(TEvent evt)
            where TEvent : IEvent;

        #endregion Methods
    }
}