namespace Nameless.BeetleTracker.EventSourcing.Domains {

    /// <summary>
    /// Extension methods for <see cref="AggregateRoot"/>
    /// </summary>
    public static class AggregateRootExtension {

        #region Public Static Methods

        /// <summary>
        /// Converts a aggregate root into a dynamic object.
        /// </summary>
        /// <param name="source">The aggregate root.</param>
        /// <returns>The dynamic instance of the aggregate root.</returns>
        public static dynamic AsDynamic(this AggregateRoot source) {
            if (source == null) { return null; }

            return new PrivateReflectionDynamicObject { RealObject = source };
        }

        #endregion Public Static Methods
    }
}