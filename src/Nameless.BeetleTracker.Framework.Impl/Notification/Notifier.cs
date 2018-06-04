using System.Collections.Generic;
using Nameless.BeetleTracker.Logging;

namespace Nameless.BeetleTracker.Notification {

    /// <summary>
    /// Default implementation of <see cref="INotifier"/>.
    /// </summary>
    public class Notifier : INotifier {

        #region Private Read-Only Fields

        private readonly IList<NotifyEntry> _entries = new List<NotifyEntry>();

        #endregion Private Read-Only Fields

        #region Public Properties

        private ILogger _logger;

        /// <summary>
        /// Gets or sets the log system.
        /// </summary>
        public ILogger Logger {
            get { return _logger ?? (_logger = NullLogger.Instance); }
            set { _logger = value ?? NullLogger.Instance; }
        }

        #endregion Public Properties

        #region INotifier Members

        /// <inheritdoc/>
        public void Add(NotifyType type, string message) {
            Logger.Information("Notification {0} message: {1}", type, message);
            _entries.Add(new NotifyEntry { Type = type, Message = message });
        }

        /// <inheritdoc/>
        public IEnumerable<NotifyEntry> GetAll() {
            return _entries;
        }

        #endregion INotifier Members
    }
}