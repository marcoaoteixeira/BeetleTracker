using System;

namespace Nameless.BeetleTracker.Settings {

    /// <summary>
    /// Defines methods to work with settings.
    /// </summary>
    public interface ISettingsStorage {

        #region Methods

        /// <summary>
        /// Loads the settings by its type.
        /// </summary>
        /// <param name="settingsType">The settings type.</param>
        /// <returns>An instance of the settings.</returns>
        object Load(Type settingsType);

        /// <summary>
        /// Saves the settings object.
        /// </summary>
        /// <param name="settings">The settings object.</param>
        void Save(object settings);

        #endregion Methods
    }
}