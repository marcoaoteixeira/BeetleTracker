namespace Nameless.BeetleTracker.Caching {

    /// <summary>
    /// Defines a cache dependency based on a physical file.
    /// </summary>
    public sealed class FileCacheDependency : CacheDependency {

        #region Public Properties

        /// <summary>
        /// Gets the dependency file path.
        /// </summary>
        public string FilePath { get; }

        #endregion Public Properties

        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of <see cref="FileCacheDependency"/>.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        public FileCacheDependency(string filePath) {
            Prevent.ParameterNullOrWhiteSpace(filePath, nameof(filePath));

            FilePath = filePath;
        }

        #endregion Public Constructors

        #region Public Static Methods

        /// <summary>
        /// Creates a new instance of <see cref="FileCacheDependency"/>.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <remarks>A new instance of <see cref="FileCacheDependency"/></remarks>
        public static CacheDependency Create(string filePath) {
            return new FileCacheDependency(filePath);
        }

        #endregion Public Static Methods
    }
}