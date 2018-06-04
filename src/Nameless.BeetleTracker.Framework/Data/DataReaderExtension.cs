using System;
using System.Data;

namespace Nameless.BeetleTracker.Data {
    /// <summary>
    /// Extension methods for <see cref="IDataReader"/>
    /// </summary>
    public static class DataReaderExtension {

        #region Public Static Methods

        /// <summary>
        /// Retrieves a GUID value or the fallback.
        /// </summary>
        /// <param name="source">The data reader instance.</param>
        /// <param name="fieldName">The field name.</param>
        /// <param name="fallback">The fallback value.</param>
        /// <returns>A GUID value.</returns>
        public static Guid GetGuidOrDefault(this IDataReader source, string fieldName, Guid fallback = default(Guid)) {
            Prevent.ParameterNullOrWhiteSpace(fieldName, nameof(fieldName));

            if (source == null) { return fallback; }

            var value = source.GetValue(fieldName);

            return (value != null && value != DBNull.Value) ? Guid.Parse(value.ToString()) : fallback;
        }

        /// <summary>
        /// Retrieves a string value or the fallback.
        /// </summary>
        /// <param name="source">The data reader instance.</param>
        /// <param name="fieldName">The field name.</param>
        /// <param name="fallback">The fallback value.</param>
        /// <returns>A string value.</returns>
        public static string GetStringOrDefault(this IDataReader source, string fieldName, string fallback = null) {
            Prevent.ParameterNullOrWhiteSpace(fieldName, nameof(fieldName));

            if (source == null) { return fallback; }

            var value = source.GetValue(fieldName);

            return (value != null && value != DBNull.Value) ? value.ToString() : fallback;
        }

        /// <summary>
        /// Retrieves an int value or the fallback.
        /// </summary>
        /// <param name="source">The data reader instance.</param>
        /// <param name="fieldName">The field name.</param>
        /// <param name="fallback">The fallback value.</param>
        /// <returns>An int value.</returns>
        public static int GetInt32OrDefault(this IDataReader source, string fieldName, int fallback = 0) {
            Prevent.ParameterNullOrWhiteSpace(fieldName, nameof(fieldName));

            if (source == null) { return fallback; }

            var value = source.GetValue(fieldName);

            return (value != null && value != DBNull.Value) ? Convert.ToInt32(value) : fallback;
        }
        /// <summary>
        /// Retrieves a boolean value or the fallback.
        /// </summary>
        /// <param name="source">The data reader instance.</param>
        /// <param name="fieldName">The field name.</param>
        /// <param name="fallback">The fallback value.</param>
        /// <returns>A boolean value.</returns>
        public static bool GetBooleanOrDefault(this IDataReader source, string fieldName, bool fallback = false) {
            Prevent.ParameterNullOrWhiteSpace(fieldName, nameof(fieldName));

            if (source == null) { return fallback; }

            var value = source.GetValue(fieldName);

            return (value != null && value != DBNull.Value) ? Convert.ToBoolean(value) : fallback;
        }
        /// <summary>
        /// Retrieves a date/time value or the fallback.
        /// </summary>
        /// <param name="source">The data reader instance.</param>
        /// <param name="fieldName">The field name.</param>
        /// <param name="fallback">The fallback value.</param>
        /// <returns>A date/time value.</returns>
        public static DateTime GetDateTimeOrDefault(this IDataReader source, string fieldName, DateTime fallback) {
            Prevent.ParameterNullOrWhiteSpace(fieldName, nameof(fieldName));

            if (source == null) { return fallback; }

            var value = source.GetValue(fieldName);

            return (value != null && value != DBNull.Value) ? Convert.ToDateTime(value) : fallback;
        }
        /// <summary>
        /// Retrieves a nullable date/time value or the fallback.
        /// </summary>
        /// <param name="source">The data reader instance.</param>
        /// <param name="fieldName">The field name.</param>
        /// <returns>A nullable date/time value.</returns>
        public static DateTime? GetDateTimeOrDefault(this IDataReader source, string fieldName) {
            Prevent.ParameterNullOrWhiteSpace(fieldName, nameof(fieldName));

            var value = source.GetValue(fieldName);

            return (value != null && value != DBNull.Value) ? Convert.ToDateTime(value) : new DateTime?();
        }
        /// <summary>
        /// Retrieves a date/time offset value or the fallback.
        /// </summary>
        /// <param name="source">The data reader instance.</param>
        /// <param name="fieldName">The field name.</param>
        /// <param name="fallback">The fallback value.</param>
        /// <returns>A date/time offset value.</returns>
        public static DateTimeOffset GetDateTimeOffsetOrDefault(this IDataReader source, string fieldName, DateTimeOffset fallback) {
            Prevent.ParameterNullOrWhiteSpace(fieldName, nameof(fieldName));

            if (source == null) { return fallback; }

            var value = source.GetValue(fieldName);

            return (value != null && value != DBNull.Value) ? DateTimeOffset.Parse(value.ToString()) : fallback;
        }
        /// <summary>
        /// Retrieves a nullable date/time offset value or the fallback.
        /// </summary>
        /// <param name="source">The data reader instance.</param>
        /// <param name="fieldName">The field name.</param>
        /// <returns>A nullable date/time offset value.</returns>
        public static DateTimeOffset? GetDateTimeOffsetOrDefault(this IDataReader source, string fieldName) {
            Prevent.ParameterNullOrWhiteSpace(fieldName, nameof(fieldName));

            var value = source.GetValue(fieldName);

            return (value != null && value != DBNull.Value) ? DateTimeOffset.Parse(value.ToString()) : new DateTimeOffset?();
        }
        /// <summary>
        /// Retrieves a by value or the fallback.
        /// </summary>
        /// <param name="source">The data reader instance.</param>
        /// <param name="fieldName">The field name.</param>
        /// <param name="fallback">The fallback value.</param>
        /// <returns>A GUID value.</returns>
        public static byte[] GetBlobOrDefault(this IDataReader source, string fieldName, byte[] fallback = null) {
            Prevent.ParameterNullOrWhiteSpace(fieldName, nameof(fieldName));

            if (source == null) { return fallback; }

            var value = source.GetValue(fieldName);

            return (value != null && value != DBNull.Value) ? (byte[])value : fallback;
        }

        #endregion Public Static Methods

        #region Private Read-Only Fields

        private static object GetValue(this IDataReader reader, string fieldName) {
            try { return reader[fieldName]; }
            catch { return null; }
        }

        #endregion
    }
}