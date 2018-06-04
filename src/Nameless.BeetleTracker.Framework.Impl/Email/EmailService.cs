using System;
using System.IO;
using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Text;
using Nameless.BeetleTracker.Logging;
using Nameless.BeetleTracker.Text;

namespace Nameless.BeetleTracker.Email {

    /// <summary>
    /// The default implementation of <see cref="IEmailService"/>
    /// </summary>
    public sealed class EmailService : IEmailService {

        #region Private Read-Only Fields

        private readonly IInterpolator _interpolator;
        private readonly SmtpClientSettings _settings;

        #endregion Private Read-Only Fields

        #region Private Fields

        private ILogger _logger;

        #endregion Private Fields

        #region Public Properties

        /// <summary>
        /// Gets or sets the Logger value.
        /// </summary>
        public ILogger Logger {
            get { return _logger ?? (_logger = NullLogger.Instance); }
            set { _logger = value ?? NullLogger.Instance; }
        }

        #endregion Public Properties

        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of <see cref="EmailService"/>.
        /// </summary>
        /// <param name="interpolator">The text interpolator</param>
        /// <param name="settings">The SMTP client settings.</param>
        public EmailService(IInterpolator interpolator, SmtpClientSettings settings) {
            Prevent.ParameterNull(interpolator, nameof(interpolator));
            Prevent.ParameterNull(settings, nameof(settings));

            _interpolator = interpolator;
            _settings = settings;
        }

        #endregion Public Constructors

        #region Private Methods

        private void InternalSend(MimeMessage message) {
            switch (_settings.DeliveryMethod) {
                case SmtpClientSettings.DeliveryMethods.PickupDirectory:
                    SendViaPickupDirectory(message);
                    break;

                case SmtpClientSettings.DeliveryMethods.Network:
                default:
                    SendViaNetwork(message);
                    break;
            }
        }

        private void SendViaPickupDirectory(MimeMessage message) {
            if (string.IsNullOrWhiteSpace(_settings.PickupDirectory) || !Directory.Exists(_settings.PickupDirectory)) {
                throw new InvalidOperationException("Pickup directory not specified or invalid.");
            }

            var path = Path.Combine(_settings.PickupDirectory, $"{Guid.NewGuid()}.eml");
            using (var stream = new FileStream(path, FileMode.Create)) {
                message.WriteTo(stream);
            }
        }

        private void SendViaNetwork(MimeMessage message) {
            SmtpClient client = null;
            try {
                client = new SmtpClient();
                client.Connect(_settings.Host, _settings.Port, _settings.EnableSsl);

                // Authenticate if possible and needed.
                if (_settings.UseCredentials && !string.IsNullOrWhiteSpace(_settings.UserName) && client.Capabilities.HasFlag(SmtpCapabilities.Authentication)) {
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    client.Authenticate(_settings.UserName, _settings.Password);
                }
                // Send message
                client.Send(message);
            }
            catch (Exception ex) { Logger.Error(ex, ex.Message); throw; }
            finally {
                if (client != null) {
                    if (client.IsConnected) {
                        client.Disconnect(quit: true);
                    }
                    client.Dispose();
                }
            }
        }

        #endregion Private Methods

        #region ISmtpClient Members

        /// <inheritdoc/>
        public void Send(Message message) {
            // Process message body
            var messageBody = message.BodyData != null
                ? _interpolator.Interpolate(message.Body, message.BodyData)
                : message.Body;
            var mail = new MimeMessage {
                Body = new TextPart(message.IsBodyHtml ? TextFormat.Html : TextFormat.Plain) {
                    Text = messageBody
                },
                Sender = MailboxAddress.Parse(message.Sender),
                Subject = message.Subject
            };

            // Set mail priority
            switch (message.Priority) {
                case MessagePriority.Low:
                    mail.Priority = MimeKit.MessagePriority.NonUrgent;
                    break;

                case MessagePriority.Medium:
                    mail.Priority = MimeKit.MessagePriority.Normal;
                    break;

                case MessagePriority.High:
                    mail.Priority = MimeKit.MessagePriority.Urgent;
                    break;
            }

            // Add recipients
            mail.From.Add(InternetAddress.Parse(message.From));
            message.Bcc.Each(_ => mail.Bcc.Add(InternetAddress.Parse(_)));
            message.Cc.Each(_ => mail.Cc.Add(InternetAddress.Parse(_)));
            message.To.Each(_ => mail.To.Add(InternetAddress.Parse(_)));

            InternalSend(mail);
        }

        #endregion ISmtpClient Members
    }
}