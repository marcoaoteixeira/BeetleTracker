using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Nameless.BeetleTracker.Localization.Json {

    /// <summary>
    /// JSON implementation of <see cref="IStringLocalizer"/>.
    /// </summary>
    public sealed class FileSystemStringLocalizer : IStringLocalizer {

        #region Private Read-Only Fields

        private readonly ConcurrentDictionary<string, Lazy<JObject>> Cache = new ConcurrentDictionary<string, Lazy<JObject>>();
        private readonly string _resourcesPath;
        private readonly string _resourceName;
        private readonly CultureInfo _culture;
        private readonly IEnumerable<string> _resourceFileLocations;

        #endregion Private Read-Only Fields

        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of <see cref="FileSystemStringLocalizer"/>
        /// </summary>
        /// <param name="resourcesPath">The resources base path.</param>
        /// <param name="resourceName">The resource name.</param>
        /// <param name="culture">The culture.</param>
        public FileSystemStringLocalizer(string resourcesPath, string resourceName, CultureInfo culture = null) {
            Prevent.ParameterNull(resourcesPath, nameof(resourcesPath));
            Prevent.ParameterNull(resourceName, nameof(resourceName));

            _resourcesPath = resourcesPath;
            _resourceName = resourceName;
            _culture = culture ?? CultureInfo.CurrentUICulture;

            // Get a list of possible resource file locations.
            _resourceFileLocations = LocalizerUtil
                .ExpandPath(_resourceName, _resourceName)
                .Select(_ => Path.Combine(_resourcesPath, _))
                .ToArray();
        }

        #endregion Public Constructors

        #region Private Methods

        private JObject GetResourceObject(CultureInfo currentCulture) {
            var cultureSuffix = string.Concat(".", currentCulture.Name);
            cultureSuffix = cultureSuffix != "." ? cultureSuffix : string.Empty;

            var lazyResource = new Lazy<JObject>(() => {
                // First attempt to find a resource file location that exists.
                string resourcePath = null;
                foreach (var resourceFileLocation in _resourceFileLocations) {
                    resourcePath = string.Concat(resourceFileLocation, cultureSuffix, ".json");
                    if (File.Exists(resourcePath)) { break; }
                    resourcePath = null;
                }
                if (resourcePath == null) { return null; }

                // Found a resource file path: attempt to parse it into a JObject.
                try {
                    using (var resourceFileStream = new FileStream(resourcePath, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize: 4096, options: FileOptions.Asynchronous | FileOptions.SequentialScan))
                    using (var resourceTextReader = new JsonTextReader(new StreamReader(resourceFileStream, Encoding.UTF8, detectEncodingFromByteOrderMarks: true))) {
                        var jObject = JObject.Load(resourceTextReader);
                        jObject.Add(nameof(LocalizableString.SearchedLocation), resourcePath);
                        return jObject;
                    }
                } catch { throw; }
            }, LazyThreadSafetyMode.ExecutionAndPublication);

            lazyResource = Cache.GetOrAdd(cultureSuffix, lazyResource);
            return lazyResource.Value;
        }

        private string[] GetCultureSuffixes(CultureInfo currentCulture) {
            // Get culture suffixes (e.g.: { "nl-NL.", "nl.", "" }).
            string[] cultureSuffixes;
            if (currentCulture != null) {
                cultureSuffixes = !currentCulture.IsNeutralCulture
                    ? cultureSuffixes = new[] { currentCulture.Name + ".", currentCulture.Parent.Name + ".", string.Empty }
                    : cultureSuffixes = new[] { currentCulture.Name + ".", string.Empty };
            } else { cultureSuffixes = new[] { string.Empty }; }
            return cultureSuffixes;
        }

        private CultureInfo[] GetCultures(CultureInfo current) => !current.IsNeutralCulture
            ? new[] { current, current.Parent }
            : new[] { current };

        private LocalizableString GetLocalizableString(string name, object[] arguments, CultureInfo culture) {
            Prevent.ParameterNull(name, nameof(name));

            // Attempt to get resource with the given name from the resource object. if not found,
            // try parent resource object until parent begets himself.
            var currentCulture = CultureInfo.CurrentCulture;
            CultureInfo previousCulture = null;
            do {
                var resourceObject = GetResourceObject(currentCulture);
                if (resourceObject != null) {
                    JToken value;
                    if (resourceObject.TryGetValue(name, out value)) {
                        var translation = value.ToString();
                        return new LocalizableString(
                            name: name,
                            value: arguments != null
                                ? string.Format(translation, arguments)
                                : translation,
                            searchedLocation: resourceObject.GetValue(nameof(LocalizableString.SearchedLocation)).ToString(),
                            resourceNotFound: false
                        );
                    }
                }

                // Consult parent culture.
                previousCulture = currentCulture;
                currentCulture = currentCulture.Parent;
            } while (previousCulture != currentCulture);
            return new LocalizableString(
                name: name,
                value: name,
                searchedLocation: null,
                resourceNotFound: true
            );
        }

        private Lazy<JObject> GetCacheItem(string key) {
            Lazy<JObject> result;
            Cache.TryGetValue(key, out result);
            return result;
        }

        #endregion Private Methods

        #region IStringLocalizer Members

        /// <inheritdoc/>
        public LocalizableString this[string name] {
            get { return GetLocalizableString(name, null /* arguments */, _culture); }
        }

        /// <inheritdoc/>
        public LocalizableString this[string name, params object[] arguments] {
            get { return GetLocalizableString(name, arguments, _culture); }
        }

        /// <inheritdoc/>
        public IEnumerable<LocalizableString> GetAllStrings(bool includeAncestorCultures) {
            var cultures = includeAncestorCultures
                ? new[] { _culture, _culture.Parent }
                : new[] { _culture };

            var result = new List<LocalizableString>();
            foreach (var culture in cultures) {
                var resource = GetResourceObject(culture);
                var items = resource
                    .Properties()
                    .Select(_ => new LocalizableString(
                        name: _.Name,
                        value: _.Value.ToString(),
                        resourceNotFound: _.Value == null
                    ));
                result.AddRange(items);
            }
            return result;
        }

        /// <inheritdoc/>
        public IStringLocalizer WithCulture(CultureInfo culture) => new FileSystemStringLocalizer(_resourcesPath, _resourceName, culture);

        #endregion IStringLocalizer Members

        #region Private Inner Classes

        private class Result {

            #region Public Properties

            public string Location { get; set; }
            public string Value { get; set; }

            #endregion Public Properties
        }

        #endregion Private Inner Classes
    }
}