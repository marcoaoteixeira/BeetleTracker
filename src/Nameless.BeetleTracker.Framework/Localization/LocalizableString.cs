using System;

namespace Nameless.BeetleTracker.Localization {

    /// <summary>
    /// A locale specific string.
    /// </summary>
    public class LocalizableString {

        #region Public Properties

        /// <summary>
        /// The name of the string in the resource it was loaded from.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The actual string.
        /// </summary>
        public string Value { get; }

        /// <summary>
        /// Whether the string was not found in a resource. If <c>true</c>, an alternate string value
        /// was used.
        /// </summary>
        public bool ResourceNotFound { get; }

        /// <summary>
        /// The location which was searched for a localization value.
        /// </summary>
        public string SearchedLocation { get; }

        #endregion Public Properties

        #region Public Constructors

        /// <summary>
        /// Creates a new <see cref="LocalizableString"/>.
        /// </summary>
        /// <param name="name">The name of the string in the resource it was loaded from.</param>
        /// <param name="value">The actual string.</param>
        public LocalizableString(string name, string value)
            : this(name, value, resourceNotFound: false) {
        }

        /// <summary>
        /// Creates a new <see cref="LocalizableString"/>.
        /// </summary>
        /// <param name="name">The name of the string in the resource it was loaded from.</param>
        /// <param name="value">The actual string.</param>
        /// <param name="resourceNotFound">
        /// Whether the string was not found in a resource. Set this to <c>true</c> to indicate an
        /// alternate string value was used.
        /// </param>
        public LocalizableString(string name, string value, bool resourceNotFound)
            : this(name, value, resourceNotFound, searchedLocation: null) {
        }

        /// <summary>
        /// Creates a new <see cref="LocalizableString"/>.
        /// </summary>
        /// <param name="name">The name of the string in the resource it was loaded from.</param>
        /// <param name="value">The actual string.</param>
        /// <param name="resourceNotFound">
        /// Whether the string was not found in a resource. Set this to <c>true</c> to indicate an
        /// alternate string value was used.
        /// </param>
        /// <param name="searchedLocation">The location which was searched for a localization value.</param>
        public LocalizableString(string name, string value, bool resourceNotFound, string searchedLocation) {
            Prevent.ParameterNull(name, nameof(name));
            Prevent.ParameterNull(name, nameof(value));

            Name = name;
            Value = value;
            ResourceNotFound = resourceNotFound;
            SearchedLocation = searchedLocation;
        }

        #endregion Public Constructors

        #region Public Implicit Operators Override

        /// <summary>
        /// Implicit operator override <see cref="LocalizableString"/> to <see cref="String"/>.
        /// </summary>
        /// <param name="localizedString"></param>
        public static implicit operator string(LocalizableString localizedString) => localizedString?.Value;

        #endregion Public Implicit Operators Override

        #region Public Override Methods

        /// <summary>
        /// Returns the actual string.
        /// </summary>
        /// <returns>The actual string.</returns>
        public override string ToString() => Value;

        #endregion Public Override Methods
    }
}