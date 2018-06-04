namespace Nameless.BeetleTracker.Environment {

    /// <summary>
    /// Defines methods to expose the application hosting environment.
    /// </summary>
    public interface IHostingEnvironment {

        #region Properties

        /// <summary>
        /// Gets the application base path.
        /// </summary>
        string ApplicationBasePath { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the application shared data.
        /// </summary>
        /// <param name="key">The data key.</param>
        /// <returns>The data.</returns>
        object GetData(string key);

        /// <summary>
        /// Sets a value to the application shared data storage.
        /// </summary>
        /// <param name="key">The data key.</param>
        /// <param name="data">The data.</param>
        void SetData(string key, object data);

        #endregion
    }
}