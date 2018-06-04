using System;

namespace Nameless.BeetleTracker.Logging {

    /// <summary>
    /// Defines the log interface.
    /// </summary>
    public interface ILogger {

        #region Methods

        /// <summary>
        /// Check if the specified log level is enabled.
        /// </summary>
        /// <param name="level">Log level.</param>
        /// <returns><c>true</c> if log level is enabled, otherwise, <c>false</c>.</returns>
        bool IsEnabled(LogLevel level);

        /// <summary>
        /// Writes the log information.
        /// </summary>
        /// <param name="level">Log level.</param>
        /// <param name="exception">The exception, if any.</param>
        /// <param name="format">The string format pattern.</param>
        /// <param name="args">The arguments for the string format pattern.</param>
        void Log(LogLevel level, Exception exception, string format, params object[] args);

        #endregion Methods
    }
}