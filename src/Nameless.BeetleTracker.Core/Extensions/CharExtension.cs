namespace Nameless.BeetleTracker {

    /// <summary>
    /// Extension methods for <see cref="char"/>.
    /// </summary>
    public static class CharExtension {

        #region Public Static Methods

        /// <summary>
        /// Whether the char is a letter between A and Z or not.
        /// </summary>
        /// <param name="source">The source <see cref="char"/>.</param>
        /// <returns><c>true</c> if is a letter, otherwise, <c>false</c>.</returns>
        public static bool IsLetter(this char source) => ('A' <= source && source <= 'Z') || ('a' <= source && source <= 'z');

        /// <summary>
        /// Whether the char is a space or not. Can be a carriage return, new line, tab, form feed or space.
        /// </summary>
        /// <param name="source">The source <see cref="char"/>.</param>
        /// <returns><c>true</c> if is a space, otherwise, <c>false</c>.</returns>
        public static bool IsSpace(this char source) => (source == '\r' || source == '\n' || source == '\t' || source == '\f' || source == ' ');

        #endregion Public Static Methods
    }
}