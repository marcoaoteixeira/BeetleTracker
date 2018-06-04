using System;
using System.Globalization;
using log4net;
using log4net.Core;

namespace Nameless.BeetleTracker.Logging {

    /// <summary>
    /// log4net implementation of <see cref="ILogger"/>
    /// </summary>
    public sealed class Logger : ILogger {

        #region Private Static Read-Only Fields

        private static readonly Level AuditLevel = new Level(2000000000, "AUDIT");

        #endregion

        #region Private Read-Only Fields

        private readonly ILog _log;

        #endregion

        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of <see cref="Logger"/>
        /// </summary>
        /// <param name="log"></param>
        public Logger(ILog log) {
            Prevent.ParameterNull(log, nameof(log));

            _log = log;
        }

        #endregion

        #region Private Methods

        private void Log(LogLevel level, Exception exception, string format) {
            switch (level) {
                case LogLevel.Debug:
                    _log.Debug(format, exception);
                    break;

                case LogLevel.Information:
                    _log.Info(format, exception);
                    break;

                case LogLevel.Warning:
                    _log.Warn(format, exception);
                    break;

                case LogLevel.Error:
                    _log.Error(format, exception);
                    break;

                case LogLevel.Fatal:
                    _log.Fatal(format, exception);
                    break;

                case LogLevel.Audit:
                    _log.Logger.Log(Type.GetType(_log.Logger.Name), AuditLevel, format, exception);
                    break;
            }
        }

        private void LogFormat(LogLevel level, string format, params object[] args) {
            switch (level) {
                case LogLevel.Debug:
                    _log.DebugFormat(CultureInfo.CurrentCulture, format, args);
                    break;

                case LogLevel.Information:
                    _log.InfoFormat(CultureInfo.CurrentCulture, format, args);
                    break;

                case LogLevel.Warning:
                    _log.WarnFormat(CultureInfo.CurrentCulture, format, args);
                    break;

                case LogLevel.Error:
                    _log.ErrorFormat(CultureInfo.CurrentCulture, format, args);
                    break;

                case LogLevel.Fatal:
                    _log.FatalFormat(CultureInfo.CurrentCulture, format, args);
                    break;

                case LogLevel.Audit:
                    _log.Logger.Log(Type.GetType(_log.Logger.Name), AuditLevel, string.Format(format, args), null);
                    break;
            }
        }

        #endregion

        #region ILogger Members

        /// <inheritdoc />
        public bool IsEnabled(LogLevel level) {
            switch (level) {
                case LogLevel.Debug:
                    return _log.IsDebugEnabled;

                case LogLevel.Information:
                    return _log.IsInfoEnabled;

                case LogLevel.Warning:
                    return _log.IsWarnEnabled;

                case LogLevel.Error:
                    return _log.IsErrorEnabled;

                case LogLevel.Fatal:
                    return _log.IsFatalEnabled;

                case LogLevel.Audit:
                    return _log.Logger.IsEnabledFor(AuditLevel);

                default:
                    return false;
            }
        }

        /// <inheritdoc />
        public void Log(LogLevel level, Exception exception, string format, params object[] args) {
            if (args != null) { LogFormat(level, format, args); } else { Log(level, exception, format); }
        }

        #endregion
    }
}