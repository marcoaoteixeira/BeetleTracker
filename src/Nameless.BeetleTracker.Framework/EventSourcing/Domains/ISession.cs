using System;

namespace Nameless.BeetleTracker.EventSourcing.Domains {

    /// <summary>
    /// Interface for session state.
    /// </summary>
    public interface ISession {

        #region Public Methods

        /// <summary>
        /// Adds an aggregate to the current session.
        /// </summary>
        /// <typeparam name="TAggregate">Type of the aggregate.</typeparam>
        /// <param name="aggregate">The instance of the aggregate.</param>
        void Add<TAggregate>(TAggregate aggregate) where TAggregate : AggregateRoot;

        /// <summary>
        /// Retrieves an aggregate in the current session.
        /// </summary>
        /// <typeparam name="TAggregate">Type of the aggregate.</typeparam>
        /// <param name="id">The aggregate ID.</param>
        /// <param name="expectedVersion">The aggregate expected version.</param>
        /// <returns>The specified aggregate.</returns>
        TAggregate Get<TAggregate>(Guid id, int? expectedVersion = null) where TAggregate : AggregateRoot;

        /// <summary>
        /// Commits all changes.
        /// </summary>
        void Commit();

        #endregion Public Methods
    }
}