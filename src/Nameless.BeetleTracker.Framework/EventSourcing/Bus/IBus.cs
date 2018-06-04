using Nameless.BeetleTracker.EventSourcing.Commands;
using Nameless.BeetleTracker.EventSourcing.Events;

namespace Nameless.BeetleTracker.EventSourcing.Bus {

    /// <summary>
    /// Interface for BUS implementation.
    /// </summary>
    public interface IBus : ICommandDispatcher, IEventPublisher { }
}