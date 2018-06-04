using System;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Proxies;
using System.Text;
using Nameless.BeetleTracker.Environment;
using Newtonsoft.Json;

namespace Nameless.BeetleTracker.Settings {

    /// <summary>
    /// Default implementation of <see cref="ISettingsStorage"/>
    /// </summary>
    public sealed class JsonSettingsStorage : ISettingsStorage {

        #region Private Read-Only Fields

        private readonly IHostingEnvironment _hostingEnvironment;

        #endregion Private Read-Only Fields

        #region Public Properties

        /// <summary>
        /// Gets the settings storage path.
        /// </summary>
        public string StoragePath { get; private set; }

        #endregion Public Properties

        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of <see cref="JsonSettingsStorage"/>
        /// </summary>
        /// <param name="hostingEnvironment"></param>
        public JsonSettingsStorage(IHostingEnvironment hostingEnvironment) {
            Prevent.ParameterNull(hostingEnvironment, nameof(hostingEnvironment));

            _hostingEnvironment = hostingEnvironment;

            Initialize();
        }

        #endregion Public Constructors

        #region Private Methods

        private void Initialize() {
            var path = ((string)_hostingEnvironment.GetData(Internals.DATADIRECTORY_KEY)) ?? typeof(ISettingsStorage).Assembly.GetDirectoryPath();

            StoragePath = Path.Combine(path, "Settings");

            if (!Directory.Exists(StoragePath)) {
                Directory.CreateDirectory(StoragePath);
            }
        }

        private SettingsBase CreateProxy(SettingsBase instance) {
            var genericType = typeof(SettingsProxy<>).MakeGenericType(instance.GetType());
            var proxyInstance = (RealProxy)Activator.CreateInstance(genericType, args: new object[] { this, instance });

            var proxy = proxyInstance.GetTransparentProxy();

            return (SettingsBase)proxy;
        }

        private string GetFilePath(Type type) {
            return Path.Combine(StoragePath, string.Concat(type.FullName, ".json"));
        }

        #endregion Private Methods

        #region ISettingsStorage Methods

        /// <inheritdoc/>
        public object Load(Type settingsType) {
            Prevent.ParameterNull(settingsType, nameof(settingsType));

            if (!typeof(SettingsBase).IsAssignableFrom(settingsType)) {
                throw new InvalidOperationException();
            }
            if (!settingsType.GetConstructors().Any(_ => _.GetParameters().Length == 0)) {
                throw new InvalidOperationException();
            }

            var filePath = GetFilePath(settingsType);
            if (!File.Exists(filePath)) {
                return CreateProxy((SettingsBase)Activator.CreateInstance(settingsType));
            }

            SettingsBase settings;
            using (var reader = new StreamReader(filePath, Encoding.UTF8)) {
                settings = (SettingsBase)JsonConvert.DeserializeObject(reader.ReadToEnd(), settingsType);
            }
            return CreateProxy(settings);
        }

        /// <inheritdoc/>
        public void Save(object settings) {
            Prevent.ParameterNull(settings, nameof(settings));

            var settingsType = settings.GetType();

            if (!typeof(SettingsBase).IsAssignableFrom(settingsType)) {
                throw new InvalidOperationException();
            }
            if (!settingsType.GetConstructors().Any(_ => _.GetParameters().Length == 0)) {
                throw new InvalidOperationException();
            }

            var filePath = GetFilePath(settingsType);
            using (var streamWriter = new StreamWriter(filePath, false /* append */, Encoding.UTF8))
            using (var jsonTextWriter = new JsonTextWriter(streamWriter)) {
                new JsonSerializer().Serialize(jsonTextWriter, settings);
            }
        }

        #endregion ISettingsStorage Methods
    }
}