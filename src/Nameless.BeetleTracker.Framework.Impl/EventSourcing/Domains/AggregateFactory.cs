using System;

namespace Nameless.BeetleTracker.EventSourcing.Domains {

    /// <summary>
    /// Default implementation of <see cref="IAggregateFactory"/>
    /// </summary>
    public sealed class AggregateFactory : IAggregateFactory {

        #region IAggregateRootFactory Members

        /// <inheritdoc />
        public TAggregate Create<TAggregate>() where TAggregate : AggregateRoot {
            try { return Activator.CreateInstance<TAggregate>(); }
            catch (MissingMethodException) { throw new MissingParameterlessConstructorException(typeof(TAggregate)); }
        }

        #endregion Internal Methods
    }
}