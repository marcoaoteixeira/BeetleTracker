using Nameless.BeetleTracker.EventSourcing.Commands;
using Nameless.BeetleTracker.EventSourcing.Events;

namespace Nameless.BeetleTracker.EventSourcing.Bus {

    /// <summary>
    /// Default implementation for <see cref="IBus"/>
    /// </summary>
    public sealed class DefaultBus : IBus {

        #region Private Read-Only Fields

        private ICommandDispatcher _dispatcher;
        private IEventPublisher _publisher;

        #endregion Private Read-Only Fields

        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of <see cref="DefaultBus"/>
        /// </summary>
        /// <param name="dispatcher">The dispatcher instance.</param>
        /// <param name="publisher">The publisher instance.</param>
        public DefaultBus(ICommandDispatcher dispatcher, IEventPublisher publisher) {
            Prevent.ParameterNull(dispatcher, nameof(dispatcher));
            Prevent.ParameterNull(publisher, nameof(publisher));

            _dispatcher = dispatcher;
            _publisher = publisher;
        }

        #endregion Public Constructors

        #region IBus Members

        /// <inheritdoc />
        public void Dispatch<TCommand>(TCommand command) where TCommand : ICommand {
            _dispatcher.Dispatch(command);
        }

        /// <inheritdoc />
        public void Publish<TEvent>(TEvent evt) where TEvent : IEvent {
            _publisher.Publish(evt);
        }

        #endregion IBus Members
    }
}