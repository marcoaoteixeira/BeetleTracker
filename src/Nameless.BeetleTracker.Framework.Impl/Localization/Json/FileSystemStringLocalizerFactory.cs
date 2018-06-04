using System;
using System.Collections.Concurrent;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using Nameless.BeetleTracker.Environment;

namespace Nameless.BeetleTracker.Localization.Json {

    /// <summary>
    /// JSON implementation of <see cref="IStringLocalizerFactory"/>.
    /// </summary>
    public sealed class FileSystemStringLocalizerFactory : IStringLocalizerFactory {

        #region Private Static Read-Only Fields

        private static readonly string[] KnownViewExtensions = new[] { ".cshtml", ".js" };
        private static readonly ConcurrentDictionary<string, IStringLocalizer> Cache = new ConcurrentDictionary<string, IStringLocalizer>();

        #endregion Private Static Read-Only Fields

        #region Private Fields

        private string _resourcesPath;

        #endregion Private Fields

        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of <see cref="FileSystemStringLocalizerFactory"/>.
        /// </summary>
        /// <param name="hostingEnvironment">The hosting environment.</param>
        /// <param name="options">The localization options.</param>
        public FileSystemStringLocalizerFactory(IHostingEnvironment hostingEnvironment, LocalizationOptions options) {
            Prevent.ParameterNull(options, nameof(options));

            var applicationBasePath = hostingEnvironment.ApplicationBasePath;
            var resourcesRelativePath = (options.ResourcesRelativePath ?? string.Empty).TrimPrefix("\\");

            _resourcesPath = Path.Combine(applicationBasePath, resourcesRelativePath);
        }

        #endregion Public Constructors

        #region IStringLocalizerFactory Members

        /// <inheritdoc/>
        public IStringLocalizer Create(Type resourceSource, CultureInfo culture = null) {
            Prevent.ParameterNull(resourceSource, nameof(resourceSource));

            var typeInfo = resourceSource.GetTypeInfo();
            // Re-root the base name if a resources path is set.
            var resourceName = Path.Combine(_resourcesPath, typeInfo.FullName);

            return Cache.GetOrAdd(typeInfo.FullName, key => new FileSystemStringLocalizer(_resourcesPath, key, culture));
        }

        /// <inheritdoc/>
        public IStringLocalizer Create(string baseName, string location, CultureInfo culture = null) {
            baseName = baseName ?? _resourcesPath;
            var resourceBaseName = location;
            var viewExtension = KnownViewExtensions.FirstOrDefault(resourceBaseName.EndsWith);
            if (viewExtension != null) {
                resourceBaseName = resourceBaseName.Substring(startIndex: 0, length: resourceBaseName.Length - viewExtension.Length);
            }
            return Cache.GetOrAdd(resourceBaseName, key => new FileSystemStringLocalizer(baseName, location, culture));
        }

        #endregion IStringLocalizerFactory Members
    }
}