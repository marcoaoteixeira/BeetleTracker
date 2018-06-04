namespace Nameless.BeetleTracker {

    /// <summary>
    /// Default implementation of <see cref="IApplicationContext"/>
    /// </summary>
    public class ApplicationContext : IApplicationContext {

        #region Public Static Read-Only Fields

        /// <summary>
        /// Gets an anonymous application context object.
        /// </summary>
        public static readonly IApplicationContext Anonymous = new ApplicationContext();

        #endregion Public Static Read-Only Fields

        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of <see cref="ApplicationContext"/>
        /// </summary>
        public ApplicationContext() { }

        /// <summary>
        /// Initializes a new instance of <see cref="ApplicationContext"/>
        /// </summary>
        /// <param name="application">The application information object.</param>
        /// <param name="user">The user information object.</param>
        public ApplicationContext(IApplicationInfo application, IUserInfo user) {
            Application = application;
            User = user;
        }

        #endregion Public Constructors

        #region IApplicationContext Members

        /// <inheritdoc/>
        public IApplicationInfo Application { get; } = ApplicationInfo.Anonymous;

        /// <inheritdoc/>
        public IUserInfo User { get; } = UserInfo.Anonymous;

        #endregion IApplicationContext Members
    }
}