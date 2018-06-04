using System;
using Nameless.BeetleTracker.Logging;
using Nameless.BeetleTracker.Notification;

namespace Nameless.BeetleTracker.ErrorHandling {

    /// <summary>
    /// Default implementation of <see cref="IExceptionPolicy"/>.
    /// </summary>
    public sealed class ExceptionPolicy : IExceptionPolicy {

        #region Private Read-Only Fields

        private readonly INotifier _notifier;

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

        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of <seealso cref="ExceptionPolicy"/>.
        /// </summary>
        /// <param name="notifier">The notification system.</param>
        public ExceptionPolicy(INotifier notifier) {
            Prevent.ParameterNull(notifier, nameof(notifier));

            _notifier = notifier;
        }

        #endregion Public Constructors

        #region Private Methods

        private void RaiseErrorNotification(Exception exception) {
            if (_notifier == null) { return; }

            _notifier.Error(exception.Message);
        }

        #endregion Private Methods

        #region IExceptionPolicy Members

        /// <inheritdoc />
        public bool Handle(object sender, Exception exception) {
            if (exception.IsFatal()) { return false; }

            Logger.Error(exception, "An unexpected exception was caught");

            do { RaiseErrorNotification(exception); }
            while ((exception = exception.InnerException) != null);

            return true;
        }

        #endregion IExceptionPolicy Members
    }
}