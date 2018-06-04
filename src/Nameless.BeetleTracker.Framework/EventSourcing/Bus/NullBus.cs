using Nameless.BeetleTracker.EventSourcing.Commands;
using Nameless.BeetleTracker.EventSourcing.Events;

namespace Nameless.BeetleTracker.EventSourcing.Bus {

    /// <summary>
    /// Null Object Pattern implementation for IBus. (see: https://en.wikipedia.org/wiki/Null_Object_pattern)
    /// </summary>
    internal sealed class NullBus : IBus {

        #region Private Static Read-Only Fields

        private static readonly IBus _instance = new NullBus();

        #endregion Private Static Read-Only Fields

        #region Public Static Properties

        /// <summary>
        /// Gets the unique instance of NullBus.
        /// </summary>
        public static IBus Instance => _instance;

        #endregion Public Static Properties

        #region Static Constructors

        // Explicit static constructor to tell the C# compiler
        // not to mark type as beforefieldinit
        static NullBus() {
        }

        #endregion Static Constructors

        #region Private Constructors

        // Prevents the class from being constructed.
        private NullBus() {
        }

        #endregion Private Constructors

        #region IBus Members

        public void Dispatch<TCommand>(TCommand command) where TCommand : ICommand {
        }

        public void Publish<TEvent>(TEvent evt) where TEvent : IEvent {
        }

        #endregion IBus Members
    }
}