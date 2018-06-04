using System;

namespace Nameless.BeetleTracker {

    /// <summary>
    /// Extension methods for <see cref="byte"/> arrays.
    /// </summary>
    public static class ByteArrayExtension {

        #region Public Static Methods

        /// <summary>
        /// Transforms an array of <see cref="byte"/>s into a hexadecimal <see cref="string"/> representation.
        /// </summary>
        /// <param name="source">The source array of <see cref="byte"/>.</param>
        /// <returns>A hexadecimal <see cref="string"/> representation of the <see cref="byte"/> array.</returns>
        public static string ToHexString(this byte[] source) {
            if (source == null) { return null; }

            return BitConverter.ToString(source).Replace("-", string.Empty);
        }

        #endregion Public Static Methods
    }
}