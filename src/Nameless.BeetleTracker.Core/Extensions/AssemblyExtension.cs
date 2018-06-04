using System;
using System.IO;
using System.Reflection;

namespace Nameless.BeetleTracker {

    /// <summary>
    /// Assembly object extension methods.
    /// </summary>
    public static class AssemblyExtension {

        #region Public Static Methods

        /// <summary>
        /// Retrieves the assembly directory path.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string GetDirectoryPath(this Assembly source) {
            if (source == null) { return null; }

            var codeBase = source.CodeBase;
            var uri = new UriBuilder(codeBase);
            var path = Uri.UnescapeDataString(uri.Path);

            return string.Concat(Path.GetDirectoryName(path), "\\");
        }

        #endregion Public Static Methods
    }
}