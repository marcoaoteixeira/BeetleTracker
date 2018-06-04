using System;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Proxies;

namespace Nameless.BeetleTracker.Settings {

    /// <summary>
    /// Proxy implementation for settings system.
    /// </summary>
    /// <typeparam name="TSettings">Type of the settings.</typeparam>
    public sealed class SettingsProxy<TSettings> : RealProxy
        where TSettings : SettingsBase {

        #region Private Read-Only Fields

        private readonly ISettingsStorage _settingsStorage;
        private readonly TSettings _settings;

        #endregion Private Read-Only Fields

        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of <see cref="SettingsProxy{TSettings}"/>
        /// </summary>
        /// <param name="settingsStorage">The settings storage instance.</param>
        /// <param name="settings">The settings instance.</param>
        public SettingsProxy(ISettingsStorage settingsStorage, TSettings settings)
            : base (typeof(TSettings)) {
            Prevent.ParameterNull(settingsStorage, nameof(settingsStorage));
            Prevent.ParameterNull(settings, nameof(settings));

            _settingsStorage = settingsStorage;
            _settings = settings;
        }

        #endregion Public Constructors

        #region Public Override Methods

        /// <inheritdoc />
        public override IMessage Invoke(IMessage msg) {
            var methodCall = (IMethodCallMessage)msg;
            var method = (MethodInfo)methodCall.MethodBase;

            try {
                if (method.Name.Equals(nameof(SettingsBase.Save))) {
                    _settingsStorage.Save(_settings);
                }
                var result = method.Invoke(_settings, methodCall.InArgs);
                return new ReturnMessage(
                    ret: result,
                    outArgs: null,
                    outArgsCount: 0,
                    callCtx: methodCall.LogicalCallContext,
                    mcm: methodCall
                );
            } catch (Exception ex) {
                return (ex is TargetInvocationException && ex.InnerException != null)
                    ? new ReturnMessage(ex.InnerException, msg as IMethodCallMessage)
                    : new ReturnMessage(ex, msg as IMethodCallMessage);
            }
        }

        #endregion Public Override Methods
    }
}