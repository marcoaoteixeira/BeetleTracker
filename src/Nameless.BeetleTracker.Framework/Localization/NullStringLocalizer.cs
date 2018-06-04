using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Nameless.BeetleTracker.Localization {

    /// <summary>
    /// Null Object Pattern implementation for IStringLocalizer. (see: https://en.wikipedia.org/wiki/Null_Object_pattern)
    /// </summary>
    public sealed class NullStringLocalizer : IStringLocalizer {

        #region Private Static Read-Only Fields

        private static readonly IStringLocalizer _instance = new NullStringLocalizer();

        #endregion Private Static Read-Only Fields

        #region Public Static Properties

        /// <summary>
        /// Gets the unique instance of NullStringLocalizer.
        /// </summary>
        public static IStringLocalizer Instance {
            get { return _instance; }
        }

        #endregion Public Static Properties

        #region Static Constructors

        // Explicit static constructor to tell the C# compiler not to mark type as beforefieldinit
        static NullStringLocalizer() {
        }

        #endregion Static Constructors

        #region Private Constructors

        // Prevents the class from being constructed.
        private NullStringLocalizer() {
        }

        #endregion Private Constructors

        #region IStringLocalizer Members

        /// <inheritdoc/>
        public LocalizableString this[string name] => new LocalizableString(name, name);

        /// <inheritdoc/>
        public LocalizableString this[string name, params object[] arguments] => new LocalizableString(name, name);

        /// <inheritdoc/>
        public IEnumerable<LocalizableString> GetAllStrings(bool includeParentCultures) => Enumerable.Empty<LocalizableString>();

        /// <inheritdoc/>
        public IStringLocalizer WithCulture(CultureInfo culture) => Instance;

        #endregion IStringLocalizer Members
    }
}