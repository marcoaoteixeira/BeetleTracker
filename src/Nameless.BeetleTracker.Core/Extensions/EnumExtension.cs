using System;
using System.Reflection;

namespace Nameless.BeetleTracker {

    /// <summary>
    /// Extension methods for <see cref="Enum"/>.
    /// </summary>
	public static class EnumExtension {

        #region Public Static Methods

        /// <summary>
        /// Gets the localized description of an <see cref="Enum"/>.
        /// </summary>
        /// <param name="source">The source <see cref="Enum"/></param>.
        /// <returns>The localized description if exists, otherwise, the <see cref="string"/> representation of the <see cref="Enum"/>.</returns>
        public static string GetDescription(this Enum source) {
            var attribute = source.GetType()
                .GetRuntimeField(source.ToString())
                .GetCustomAttribute<DescriptionAttribute>(inherit: false);

            return attribute != null ? attribute.Description : source.ToString();
        }

        #endregion Public Static Methods
    }
}