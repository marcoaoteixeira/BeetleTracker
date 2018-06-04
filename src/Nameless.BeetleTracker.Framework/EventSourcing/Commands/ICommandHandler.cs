using Nameless.BeetleTracker.EventSourcing.Messaging;

namespace Nameless.BeetleTracker.EventSourcing.Commands {

    /// <summary>
    /// Command handler interface.
    /// </summary>
    /// <typeparam name="TCommand">Type of the command.</typeparam>
    public interface ICommandHandler<in TCommand> : IHandler<TCommand> where TCommand : ICommand { }
}