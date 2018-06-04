using System;

namespace Nameless.BeetleTracker.Services {

    /// <summary>
    /// Provides the current UTC <see cref="DateTime"/>.
    /// This service should be used whenever the current date and time are needed, instead of <seealso cref="DateTime"/> directly.
    /// It also makes implementations more testable, as time can be mocked.
    /// </summary>
    public interface IClock {

        #region Properties

        /// <summary>
        /// Gets the current <see cref="DateTime"/> of the system, expressed in Utc
        /// </summary>
        DateTime UtcNow { get; }

        /// <summary>
        /// Gets the current <see cref="DateTimeOffset"/> of the system.
        /// </summary>
        DateTimeOffset OffsetUtcNow { get; }

        #endregion Properties
    }
}