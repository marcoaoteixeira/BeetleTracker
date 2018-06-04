using Nameless.BeetleTracker.Settings;

namespace Nameless.BeetleTracker.Email {

    /// <summary>
    /// The configuration for SMTP client.
    /// </summary>
    public sealed class SmtpClientSettings : SettingsBase {

        #region Public Enumerators

        /// <summary>
        /// Enumerates the delivery methods.
        /// </summary>
        public enum DeliveryMethods {
            /// <summary>
            /// Network
            /// </summary>
            Network = 0,

            /// <summary>
            /// Pickup Directory
            /// </summary>
            PickupDirectory = 1
        }

        #endregion Public Enumerators

        #region Public Properties

        /// <summary>
        /// Gets or sets the SMTP server address.
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// Whether if will use port, or not use, to connecto to the SMTP service.
        /// </summary>
        public bool UsePort { get; set; }

        /// <summary>
        /// Gets or sets the SMTP server port.
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// Gets or sets if should use credentials.
        /// </summary>
        public bool UseCredentials { get; set; }

        /// <summary>
        /// Gets or sets the user name credential.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the password credential.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets if should enable SSL.
        /// </summary>
        public bool EnableSsl { get; set; }

        /// <summary>
        /// Gets or sets the delivery method
        /// </summary>
        public DeliveryMethods DeliveryMethod { get; set; }

        /// <summary>
        /// Gets or sets the pickup directory
        /// </summary>
        public string PickupDirectory { get; set; }

        #endregion Public Properties
    }
}