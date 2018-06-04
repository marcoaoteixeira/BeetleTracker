using System;
using System.Reflection;

namespace Nameless.BeetleTracker {

    /// <summary>
    /// Specifies a localized description for a property or event.
    /// </summary>
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    public sealed class DescriptionAttribute : System.ComponentModel.DescriptionAttribute {

        #region Public Properties

        /// <summary>
        /// Gets or sets the resource type key.
        /// </summary>
        public string ResourceKey { get; set; }

        /// <summary>
        /// Gets or sets the resource type.
        /// </summary>
		public Type ResourceType { get; set; }

        #endregion Public Properties

        #region Public Override Properties

        /// <inheritdoc />
        public override string Description {
            get { return GetDescription(); }
        }

        #endregion Public Override Properties

        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of <see cref="DescriptionAttribute"/>.
        /// </summary>
        /// <param name="fallback">A fall back for the description.</param>
        public DescriptionAttribute(string fallback = null)
            : base(fallback) { }

        #endregion Public Constructors

        #region Private Methods

        private string GetDescription() {
            if (string.IsNullOrWhiteSpace(ResourceKey) || ResourceType == null) {
                return base.Description;
            }

            var property = ResourceType.GetTypeInfo().GetProperty(ResourceKey, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);

            return property != null
                ? property.GetValue(null, null).ToString()
                : base.Description;
        }

        #endregion Private Methods
    }
}