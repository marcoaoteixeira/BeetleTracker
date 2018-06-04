using System;
using System.Globalization;

namespace Nameless.BeetleTracker.Localization {

    /// <summary>
    /// Represents a factory that creates <see cref="IStringLocalizer"/> instances.
    /// </summary>
    public interface IStringLocalizerFactory {

        #region Methods

        /// <summary>
        /// Creates an <see cref="IStringLocalizer"/> using the <see cref="System.Reflection.Assembly"/> and
        /// <see cref="Type.FullName"/> of the specified <see cref="Type"/>.
        /// </summary>
        /// <param name="resourceSource">The <see cref="Type"/>.</param>
        /// <param name="culture">The culture.</param>
        /// <returns>The <see cref="IStringLocalizer"/>.</returns>
        IStringLocalizer Create(Type resourceSource, CultureInfo culture = null);

        /// <summary>
        /// Creates an <see cref="IStringLocalizer"/>.
        /// </summary>
        /// <param name="baseName">The base name of the resource to load strings from.</param>
        /// <param name="location">The location to load resources from.</param>
        /// <param name="culture">The culture.</param>
        /// <returns>The <see cref="IStringLocalizer"/>.</returns>
        IStringLocalizer Create(string baseName, string location, CultureInfo culture = null);

        #endregion Methods
    }
}