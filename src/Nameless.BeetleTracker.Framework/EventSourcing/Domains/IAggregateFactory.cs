namespace Nameless.BeetleTracker.EventSourcing.Domains {

    /// <summary>
    /// Interface for a aggregate factory.
    /// </summary>
    public interface IAggregateFactory {

        #region Methods

        /// <summary>
        /// Creates an aggregate.
        /// </summary>
        /// <typeparam name="TAggregate">Type of the aggregate.</typeparam>
        /// <returns>The aggregate instance.</returns>
        TAggregate Create<TAggregate>() where TAggregate : AggregateRoot;

        #endregion Methods
    }
}