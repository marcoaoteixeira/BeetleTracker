using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using Nameless.BeetleTracker.Logging;

namespace Nameless.BeetleTracker.Data {

    /// <summary>
    /// Default implementation of <see cref="IDatabase"/>.
    /// </summary>
    public sealed class Database : IDatabase, IDisposable {

        #region Private Read-Only Fields

        private readonly IDbProviderSelector _dbProviderSelector;
        private readonly DatabaseSettings _databaseSettings;

        #endregion Private Read-Only Fields

        #region Private Fields

        private DbProviderFactory _factory;
        private DbConnection _connection;
        private bool _disposed;

        #endregion Private Fields

        #region Public Properties

        private ILogger _logger;

        /// <summary>
        /// Gets or sets the logger instance.
        /// </summary>
        public ILogger Logger {
            get { return _logger ?? NullLogger.Instance; }
            set { _logger = value ?? NullLogger.Instance; }
        }

        #endregion Public Properties

        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of <see cref="Database"/>.
        /// </summary>
        /// <param name="dbProviderSelector">Database provider selector instance.</param>
        /// <param name="databaseSettings">The configuration section.</param>
        public Database(IDbProviderSelector dbProviderSelector, DatabaseSettings databaseSettings) {
            Prevent.ParameterNull(dbProviderSelector, nameof(dbProviderSelector));
            Prevent.ParameterNull(databaseSettings, nameof(databaseSettings));

            _dbProviderSelector = dbProviderSelector;
            _databaseSettings = databaseSettings;
        }

        #endregion Public Constructors

        #region Destructor

        /// <summary>
        /// Destructor
        /// </summary>
        ~Database() {
            Dispose(disposing: false);
        }

        #endregion Destructor

        #region Private Methods

        private DbProviderFactory GetFactory() {
            return _factory ?? (_factory = _dbProviderSelector.GetFactory(_databaseSettings.ProviderName));
        }

        private DbConnection GetConnection() {
            try {
                if (_connection == null) {
                    _connection = GetFactory().CreateConnection();
                    _connection.ConnectionString = _databaseSettings.ConnectionString;
                    _connection.Open();
                }
            } catch (Exception ex) { Logger.Error(ex, ex.Message); throw; }

            return _connection;
        }

        private DbParameter ConvertParameter(Parameter parameter) {
            var result = GetFactory().CreateParameter();
            result.ParameterName = (!parameter.Name.StartsWith("@") ? string.Concat("@", parameter.Name) : parameter.Name);
            result.DbType = parameter.Type;
            result.Direction = parameter.Direction;
            result.Value = (parameter.Value ?? DBNull.Value);
            return result;
        }

        private void EnsureAccessBlockedAfterDispose() {
            if (_disposed) { throw new ObjectDisposedException(GetType().Name); }
        }

        private void Dispose(bool disposing) {
            if (_disposed) { return; }
            if (disposing) {
                if (_connection != null) {
                    if (_connection.State == ConnectionState.Open) {
                        _connection.Close();
                    }
                    _connection.Dispose();
                }
            }

            _factory = null;
            _connection = null;
            _disposed = true;
        }

        private void PrepareCommand(DbCommand command, string commandText, CommandType commandType, Parameter[] parameters) {
            command.CommandText = commandText;
            command.CommandType = commandType;

            parameters.Each(parameter => command.Parameters.Add(ConvertParameter(parameter)));
        }

        private object Execute(string commandText, CommandType commandType, Parameter[] parameters, bool scalar) {
            try {
                using (var command = GetConnection().CreateCommand()) {
                    PrepareCommand(command, commandText, commandType, parameters);

                    var result = scalar
                        ? command.ExecuteScalar()
                        : command.ExecuteNonQuery();

                    command.Parameters.OfType<DbParameter>()
                        .Where(dbParameter => dbParameter.Direction != ParameterDirection.Input)
                        .Each(dbParameter => {
                            parameters
                                .Single(parameter =>
                                    parameter.Name == dbParameter.ParameterName &&
                                    parameter.Direction == dbParameter.Direction)
                                .Value = dbParameter.Value;
                        });

                    return result;
                }
            } catch (Exception ex) { Logger.Error(ex, ex.Message); throw; }
        }

        #endregion Private Methods

        #region IDatabase Members

        /// <inheritdoc/>
        public IDbConnection Connection {
            get { return _connection; }
        }

        /// <inheritdoc/>
        public int ExecuteNonQuery(string commandText, CommandType commandType = CommandType.Text, params Parameter[] parameters) {
            EnsureAccessBlockedAfterDispose();

            return (int)Execute(commandText, commandType, parameters, false);
        }

        /// <inheritdoc/>
        public IEnumerable<TResult> ExecuteReader<TResult>(string commandText, Func<IDataReader, TResult> mapper, CommandType commandType = CommandType.Text, params Parameter[] parameters) {
            EnsureAccessBlockedAfterDispose();

            using (var command = GetConnection().CreateCommand()) {
                PrepareCommand(command, commandText, commandType, parameters);

                IDataReader reader;
                try { reader = command.ExecuteReader(); } catch (Exception ex) { Logger.Error(ex, ex.Message); throw; }
                using (reader) {
                    while (reader.Read()) {
                        yield return mapper(reader);
                    }
                }
            }
        }

        /// <inheritdoc/>
        public object ExecuteScalar(string commandText, CommandType commandType = CommandType.Text, params Parameter[] parameters) {
            EnsureAccessBlockedAfterDispose();

            return Execute(commandText, commandType, parameters, true);
        }

        #endregion IDatabase Members

        #region IDisposable Members

        /// <inheritdoc/>
        public void Dispose() {
            GC.SuppressFinalize(this);
            Dispose(true);
        }

        #endregion IDisposable Members
    }
}