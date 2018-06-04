using System;
using System.Reflection;

namespace Nameless.BeetleTracker.EventSourcing.Events {

    /// <summary>
    /// Extension methods for <see cref="IEventPublisher"/>.
    /// </summary>
    public static class EventPublisherExtension {

        #region Public Static Methods

        /// <summary>
        /// Publishes an event.
        /// </summary>
        /// <param name="source">The source <see cref="IEventPublisher"/>.</param>
        /// <param name="eventType">The event type.</param>
        /// <param name="evt">The event.</param>
        public static void Publish(this IEventPublisher source, Type eventType, IEvent evt) {
            if (source == null) { return; }

            Prevent.ParameterNull(eventType, nameof(eventType));
            Prevent.ParameterTypeNotAssignableFrom(typeof(IEvent), eventType);
            Prevent.ParameterNull(evt, nameof(evt));

            typeof(IEventPublisher)
                .GetMethod(nameof(IEventPublisher.Publish))
                .MakeGenericMethod(new[] { eventType })
                .Invoke(source, new object[] { evt });
        }

        #endregion Public Static Methods
    }
}