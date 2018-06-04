using System;
using System.Data;
using System.Linq;

namespace Nameless.BeetleTracker.Data {

    /// <summary>
    /// Extension methods for <see cref="IDatabase"/>
    /// </summary>
    public static class DatabaseExtension {

        #region Public Static Methods

        /// <summary>
        /// Executes a reader query against the data base, and returns only one result.
        /// If more than one result was found, throws exception.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="source">The <see cref="IDatabase"/> instance.</param>
        /// <param name="commandText">The command text.</param>
        /// <param name="commandType">The command type.</param>
        /// <param name="mapper">The mapper for result projection.</param>
        /// <param name="parameters">The command parameters.</param>
        /// <returns>A instance of <typeparamref name="TResult"/>.</returns>
        public static TResult ExecuteReaderSingle<TResult>(this IDatabase source, string commandText, Func<IDataReader, TResult> mapper, CommandType commandType = CommandType.Text, params Parameter[] parameters) {
            if (source == null) { return default(TResult); }

            return source
                .ExecuteReader(commandText, mapper, commandType, parameters)
                .SingleOrDefault();
        }

        #endregion Public Static Methods
    }
}