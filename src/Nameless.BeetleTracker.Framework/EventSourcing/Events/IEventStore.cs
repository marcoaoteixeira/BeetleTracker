using System;
using System.Collections.Generic;

namespace Nameless.BeetleTracker.EventSourcing.Events {

    /// <summary>
    /// Interface for event store implementation.
    /// </summary>
    public interface IEventStore {

        #region Methods

        /// <summary>
        /// Saves the events.
        /// </summary>
        /// <param name="evts">The collection event.</param>
        void Save(params IEvent[] evts);

        /// <summary>
        /// Retrieves all events for the aggregate ID, from specific version.
        /// </summary>
        /// <param name="aggregateID">The aggregate ID.</param>
        /// <param name="fromVersion">Specified version to find.</param>
        /// <returns>A collection of <see cref="IEvent"/>.</returns>
        IEnumerable<IEvent> Get(Guid aggregateID, int fromVersion);

        #endregion Methods
    }
}