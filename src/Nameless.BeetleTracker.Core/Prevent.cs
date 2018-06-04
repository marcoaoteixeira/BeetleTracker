using System;
using System.Diagnostics;
using System.Reflection;

namespace Nameless.BeetleTracker {

    /// <summary>
    /// A static helper class that includes various parameter checking routines.
    /// </summary>
    public static class Prevent {

        #region Public Static Methods

        /// <summary>
        /// Ensures that the parameter value is not <c>null</c>.
        /// </summary>
        /// <param name="parameterValue">The parameter value.</param>
        /// <param name="parameterName">The parameter name.</param>
        /// <exception cref="ArgumentNullException">
        /// if <paramref name="parameterValue"/> is <c>null</c>.
        /// </exception>
        [DebuggerStepThrough]
        public static void ParameterNull(object parameterValue, string parameterName) {
            CheckParameterName(parameterName);

            if (parameterValue == null) {
                throw new ArgumentNullException(parameterName);
            }
        }

        /// <summary>
        /// Ensures that the parameter value cannot be <c>null</c>, empty or white spaces.
        /// </summary>
        /// <param name="parameterValue">The parameter value.</param>
        /// <param name="parameterName">The parameter name.</param>
        /// <exception cref="ArgumentNullException">
        /// if <paramref name="parameterValue"/> is <c>null</c>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// if <paramref name="parameterValue"/> is empty or white space.
        /// </exception>
        [DebuggerStepThrough]
        public static void ParameterNullOrWhiteSpace(string parameterValue, string parameterName) {
            CheckParameterName(parameterName);

            ParameterNull(parameterValue, parameterName);

            if (string.IsNullOrWhiteSpace(parameterValue)) {
                throw new ArgumentException(Properties.Resources.ParameterCannotBeNullEmptyOrWhiteSpaces, parameterName);
            }
        }

        /// <summary>
        /// Ensure that the type is assignable from the inherit type.
        /// </summary>
        /// <param name="currentType">The type.</param>
        /// <param name="specificType">The inherit type.</param>
        /// <exception cref="ArgumentNullException">if <paramref name="currentType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">if <paramref name="specificType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">
        /// if <paramref name="currentType"/> is not assignable from <paramref name="specificType"/>.
        /// </exception>
        [DebuggerStepThrough]
        public static void ParameterTypeNotAssignableFrom(Type currentType, Type specificType) {
            ParameterNull(currentType, nameof(currentType));
            ParameterNull(specificType, nameof(specificType));

            if (!currentType.GetTypeInfo().IsAssignableFrom(specificType.GetTypeInfo())) {
                throw new ArgumentException(string.Format(Properties.Resources.ParameterMustBeAssignableToSpecificType, currentType, specificType));
            }
        }

        #endregion Public Static Methods

        #region Private Static Methods

        private static void CheckParameterName(string parameterName) {
            if (string.IsNullOrWhiteSpace(parameterName)) {
                throw new ArgumentException(Properties.Resources.ParameterCannotBeNullEmptyOrWhiteSpaces, nameof(parameterName));
            }
        }

        #endregion Private Static Methods
    }
}