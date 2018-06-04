namespace Nameless.BeetleTracker.Settings {

    /// <summary>
    /// Extension methods for <see cref="ISettingsStorage"/>.
    /// </summary>
    public static class SettingsStorageExtension {

        #region Public Static Methods

        /// <summary>
        /// Loads settings by its type.
        /// </summary>
        /// <typeparam name="TSettings">Type of the settings.</typeparam>
        /// <param name="source">The settings storage instance.</param>
        /// <returns>An instance of the settings.</returns>
        public static TSettings Load<TSettings>(this ISettingsStorage source) where TSettings : SettingsBase, new() {
            if (source == null) { return null; }

            return (TSettings)source.Load(typeof(TSettings));
        }

        #endregion Public Static Methods
    }
}