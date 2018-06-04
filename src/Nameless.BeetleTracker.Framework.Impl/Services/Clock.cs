using System;

namespace Nameless.BeetleTracker.Services {

    /// <inheritdoc/>
    public class Clock : IClock {

        #region IClock Members

        /// <inheritdoc/>
        public DateTime UtcNow {
            get { return DateTime.UtcNow; }
        }

        /// <inheritdoc/>
        public DateTimeOffset OffsetUtcNow {
            get { return DateTimeOffset.UtcNow; }
        }

        #endregion IClock Members
    }
}