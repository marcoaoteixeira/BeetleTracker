using System;
using System.Collections.Generic;
using System.Data;

namespace Nameless.BeetleTracker.Data {

    /// <summary>
    /// Defines methods/properties/events to implement a database accessor to work with ADO.Net
    /// </summary>
    public interface IDatabase {

        #region Properties

        /// <summary>
        /// Gets the current data base connection.
        /// </summary>
        IDbConnection Connection { get; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Executes a not-query command against the data base.
        /// </summary>
        /// <param name="commandText">The command text.</param>
        /// <param name="commandType">The command type. Default is <see cref="CommandType.Text"/>.</param>
        /// <param name="parameters">The command parameters.</param>
        /// <returns>An <see cref="int"/> value representing the result of this execution.</returns>
        int ExecuteNonQuery(string commandText, CommandType commandType = CommandType.Text, params Parameter[] parameters);

        /// <summary>
        /// Executes a reader query against the data base.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="commandText">The command text.</param>
        /// <param name="commandType">The command type.</param>
        /// <param name="mapper">The mapper for result projection.</param>
        /// <param name="parameters">The command parameters.</param>
        /// <returns>A instance of <typeparamref name="TResult"/>.</returns>
        IEnumerable<TResult> ExecuteReader<TResult>(string commandText, Func<IDataReader, TResult> mapper, CommandType commandType = CommandType.Text, params Parameter[] parameters);

        /// <summary>
        /// Executes a scalar command against the data base.
        /// </summary>
        /// <param name="commandText">The command text.</param>
        /// <param name="commandType">The command type.</param>
        /// <param name="parameters">The command parameters.</param>
        /// <returns>A <see cref="object"/> representing the result of this execution.</returns>
        object ExecuteScalar(string commandText, CommandType commandType = CommandType.Text, params Parameter[] parameters);

        #endregion Methods
    }
}