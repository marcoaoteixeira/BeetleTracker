using Nameless.BeetleTracker.EventSourcing.Messaging;

namespace Nameless.BeetleTracker.EventSourcing.Events {

    /// <summary>
    /// Defines methods/properties/events to implement an event handler.
    /// </summary>
    /// <typeparam name="TEvent">Type of the event.</typeparam>
    public interface IEventHandler<in TEvent> : IHandler<TEvent> where TEvent : IEvent { }
}