using System.Collections.Generic;
using System.Globalization;

namespace Nameless.BeetleTracker.Localization {

    /// <summary>
    /// Represents a service that provides localized strings.
    /// </summary>
    public interface IStringLocalizer {

        #region Properties

        /// <summary>
        /// Gets the string resource with the given name.
        /// </summary>
        /// <param name="name">The name of the string resource.</param>
        /// <returns>The string resource as a <see cref="LocalizableString"/>.</returns>
        LocalizableString this[string name] { get; }

        /// <summary>
        /// Gets the string resource with the given name and formatted with the supplied arguments.
        /// </summary>
        /// <param name="name">The name of the string resource.</param>
        /// <param name="arguments">The values to format the string with.</param>
        /// <returns>The formatted string resource as a <see cref="LocalizableString"/>.</returns>
        LocalizableString this[string name, params object[] arguments] { get; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Gets all string resources.
        /// </summary>
        /// <param name="includeParentCultures">
        /// A <see cref="System.Boolean"/> indicating whether to include strings from parent cultures.
        /// </param>
        /// <returns>The strings.</returns>
        IEnumerable<LocalizableString> GetAllStrings(bool includeParentCultures);

        /// <summary>
        /// Creates a new <see cref="IStringLocalizer"/> for a specific <see cref="CultureInfo"/>.
        /// </summary>
        /// <param name="culture">The <see cref="CultureInfo"/> to use.</param>
        /// <returns>A culture-specific <see cref="IStringLocalizer"/>.</returns>
        IStringLocalizer WithCulture(CultureInfo culture);

        #endregion Methods
    }

    /// <summary>
    /// Represents an <see cref="IStringLocalizer"/> that provides strings for <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The <see cref="System.Type"/> to provide strings for.</typeparam>
    public interface IStringLocalizer<T> : IStringLocalizer {
    }
}