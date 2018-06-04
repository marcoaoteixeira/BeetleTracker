namespace Nameless.BeetleTracker.Localization {

    /// <summary>
    /// Default implementation of <see cref="IPluralStringLocalizer{T}"/>.
    /// </summary>
    /// <typeparam name="T">Type scope.</typeparam>
    public class PluralStringLocalizer<T> : IPluralStringLocalizer<T> {

        #region Private Read-Only Fields

        private readonly IStringLocalizer<T> _stringLocalizer;

        #endregion Private Read-Only Fields

        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of <see cref="PluralStringLocalizer{T}"/>.
        /// </summary>
        /// <param name="stringLocalizer">The current string localizer.</param>
        public PluralStringLocalizer(IStringLocalizer<T> stringLocalizer) {
            Prevent.ParameterNull(stringLocalizer, nameof(stringLocalizer));

            _stringLocalizer = stringLocalizer;
        }

        #endregion Public Constructors

        #region IPluralStringLocalizer<T> Members

        /// <inheritdoc />
        public LocalizableString this[string key, string pluralName, int count, params object[] arguments] => count > 1
            ? _stringLocalizer[pluralName, arguments]
            : _stringLocalizer[key, arguments];

        #endregion IPluralStringLocalizer<T> Members
    }
}