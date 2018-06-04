using System;

namespace Nameless.BeetleTracker.Settings {

    /// <summary>
    /// Base class for settings.
    /// </summary>
    public abstract class SettingsBase : MarshalByRefObject {

        #region Public Methods

        /// <summary>
        /// Saves the settings.
        /// </summary>
        /// <remarks>Only for proxy use.</remarks>
        public void Save() { }

        #endregion Public Methods
    }
}