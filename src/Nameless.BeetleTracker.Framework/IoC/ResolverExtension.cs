namespace Nameless.BeetleTracker.IoC {

    /// <summary>
    /// Extension methods for <see cref="IResolver"/>.
    /// </summary>
    public static class ResolverExtension {

        #region Public Static Methods

        /// <summary>
        /// Resolves a service by its type.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <param name="source">The implemented <see cref="IResolver"/> instance.</param>
        /// <param name="name">The name of the service, if any.</param>
        /// <returns>An instance of the service.</returns>
        public static TService Resolve<TService>(this IResolver source, string name = null) {
            if (source == null) { return default(TService); }

            return (TService)source.Resolve(serviceType: typeof(TService), name: name);
        }

        #endregion Public Static Methods
    }
}