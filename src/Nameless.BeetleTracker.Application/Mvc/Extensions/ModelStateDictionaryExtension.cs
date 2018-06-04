using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace Nameless.BeetleTracker.Mvc {

    /// <summary>
    /// Extension methods for <see cref="ModelStateDictionary"/>.
    /// </summary>
    public static class ModelStateDictionaryExtension {

        #region Public Static Methods

        /// <summary>
        /// Adds the <see cref="IdentityResult"/> errors to the <see cref="ModelStateDictionary"/>.
        /// </summary>
        /// <param name="source">The source <see cref="ModelStateDictionary"/>.</param>
        /// <param name="result">The <see cref="IdentityResult"/> instance.</param>
        public static void AddErrosFromIdentityResult(this ModelStateDictionary source, IdentityResult result) {
            if (source == null) { return; }

            result.Errors.Each(_ => source.AddModelError(string.Empty, _));
        }

        #endregion Public Static Methods
    }
}