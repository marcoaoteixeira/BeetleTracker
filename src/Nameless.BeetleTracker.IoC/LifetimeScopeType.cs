namespace Nameless.BeetleTracker.IoC {

    /// <summary>
    /// Life time scope types.
    /// </summary>
    public enum LifetimeScopeType : int {

        /// <summary>
        /// Singleton life time.
        /// </summary>
        Singleton,

        /// <summary>
        /// Every request to the object gets a new instance.
        /// </summary>
        Transient,

        /// <summary>
        /// Life time per request (Web).
        /// </summary>
        PerRequest,

        /// <summary>
        /// Life time per container scope.
        /// </summary>
        PerScope
    }
}