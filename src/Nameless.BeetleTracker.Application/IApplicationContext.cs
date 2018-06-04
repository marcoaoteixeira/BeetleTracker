namespace Nameless.BeetleTracker {

    /// <summary>
    /// Application context
    /// </summary>
    public interface IApplicationContext {

        #region Properties

        /// <summary>
        /// Gets the application info object.
        /// </summary>
        IApplicationInfo Application { get; }
        /// <summary>
        /// Gets the user info object.
        /// </summary>
        IUserInfo User { get; }

        #endregion Properties
    }
}