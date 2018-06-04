namespace Nameless.BeetleTracker.IoC {

    /// <summary>
    /// Composition root interface.
    /// </summary>
    public interface ICompositionRoot {

        #region Properties

        /// <summary>
        /// Gets an instance of the implemented <see cref="IResolver"/>
        /// </summary>
        IResolver Resolver { get; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Composes the root.
        /// </summary>
        /// <param name="registrations">Registrations.</param>
        void Compose(params IServiceRegistration[] registrations);

        /// <summary>
        /// Start up the composition root.
        /// </summary>
        void StartUp();

        /// <summary>
        /// Tears down the composition root.
        /// </summary>
        void TearDown();

        #endregion Methods
    }
}