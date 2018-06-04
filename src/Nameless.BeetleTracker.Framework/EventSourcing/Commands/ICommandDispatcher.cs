namespace Nameless.BeetleTracker.EventSourcing.Commands {

    /// <summary>
    /// Interface for <see cref="ICommand"/> dispatching.
    /// </summary>
    public interface ICommandDispatcher {

        #region Methods

        /// <summary>
        /// Dispatches a command.
        /// </summary>
        /// <typeparam name="TCommand">Type of the command.</typeparam>
        /// <param name="command">The command.</param>
        void Dispatch<TCommand>(TCommand command) where TCommand : ICommand;

        #endregion Methods
    }
}