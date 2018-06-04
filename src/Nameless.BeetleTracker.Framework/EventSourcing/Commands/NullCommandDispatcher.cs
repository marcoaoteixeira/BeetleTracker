namespace Nameless.BeetleTracker.EventSourcing.Commands {

    /// <summary>
    /// Null Object Pattern implementation for ICommandSender. (see: https://en.wikipedia.org/wiki/Null_Object_pattern)
    /// </summary>
    internal sealed class NullCommandDispatcher : ICommandDispatcher {

        #region Private Static Read-Only Fields

        private static readonly ICommandDispatcher _instance = new NullCommandDispatcher();

        #endregion Private Static Read-Only Fields

        #region Public Static Properties

        /// <summary>
        /// Gets the unique instance of NullCommandSender.
        /// </summary>
        public static ICommandDispatcher Instance => _instance;

        #endregion Public Static Properties

        #region Static Constructors

        // Explicit static constructor to tell the C# compiler
        // not to mark type as beforefieldinit
        static NullCommandDispatcher() {
        }

        #endregion Static Constructors

        #region Private Constructors

        // Prevents the class from being constructed.
        private NullCommandDispatcher() {
        }

        #endregion Private Constructors

        #region ICommandSender Members

        public void Dispatch<TCommand>(TCommand command)
            where TCommand : ICommand {
        }

        #endregion ICommandSender Members
    }
}