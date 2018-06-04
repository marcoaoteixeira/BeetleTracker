using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Nameless.BeetleTracker.Localization;
using Nameless.BeetleTracker.Notification;

namespace Nameless.BeetleTracker.Mvc.Filters {

    /// <summary>
    /// Notify implementation of <see cref="IActionFilter"/>.
    /// </summary>
    public sealed class NotifyFilter : IActionFilter, IResultFilter {

        #region Public Static Read-Only Fields

        /// <summary>
        /// Gets the temp data key for notification messages.
        /// </summary>
        public static readonly string TempDataNotifications = "Notifications";

        #endregion Public Static Read-Only Fields

        #region Private Read-Only Fields

        private readonly INotifier _notifier;

        #endregion Private Read-Only Fields

        #region Public Properties
        
        private IStringLocalizer _localizer;
        /// <summary>
        /// Gets or sets the localizer.
        /// </summary>
        public IStringLocalizer T {
            get { return _localizer ?? (_localizer = NullStringLocalizer.Instance); }
            set { _localizer = value ?? NullStringLocalizer.Instance; }
        }

        #endregion Public Properties

        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of <see cref="NotifyFilter"/>.
        /// </summary>
        /// <param name="notifier">An instance of <see cref="INotifier"/>.</param>
        public NotifyFilter(INotifier notifier) {
            Prevent.ParameterNull(notifier, nameof(notifier));

            _notifier = notifier;
        }

        #endregion Public Constructors

        #region IActionFilter Members

        /// <inheritdoc/>
        public void OnActionExecuted(ActionExecutedContext filterContext) {
            // don't touch temp data if there's no work to perform
            if (!_notifier.GetAll().Any()) { return; }

            var tempData = filterContext.Controller.TempData;

            // initialize writer with current data
            var notifications = new StringBuilder();
            if (tempData.ContainsKey(TempDataNotifications)) {
                notifications.Append(tempData[TempDataNotifications]);
            }

            // accumulate messages, one line per message
            foreach (var entry in _notifier.GetAll()) {
                notifications.Append(Convert.ToString(entry.Type))
                    .Append(':')
                    .AppendLine(entry.Message.ToString())
                    .AppendLine("-");
            }

            // assign values into temp data string data type used instead of complex array to be session-friendly
            tempData[TempDataNotifications] = notifications.ToString();
        }

        /// <inheritdoc/>
        public void OnActionExecuting(ActionExecutingContext filterContext) {
            var notifications = Convert.ToString(filterContext.Controller.TempData[TempDataNotifications]);
            if (string.IsNullOrEmpty(notifications)) {
                return;
            }

            var splitter = string.Concat(System.Environment.NewLine, "-", System.Environment.NewLine);
            var notificationEntries = new List<NotifyEntry>();
            foreach (var line in notifications.Split(new[] { splitter }, StringSplitOptions.RemoveEmptyEntries)) {
                var delimiterIndex = line.IndexOf(':');
                if (delimiterIndex != -1) {
                    var type = (NotifyType)Enum.Parse(typeof(NotifyType), line.Substring(0, delimiterIndex));
                    var message = T[line.Substring(delimiterIndex + 1)];
                    if (!notificationEntries.Any(entry => entry.Message == message.Value)) {
                        notificationEntries.Add(new NotifyEntry { Type = type, Message = message.Value });
                    }
                } else {
                    var message = T[line.Substring(delimiterIndex + 1)];
                    if (!notificationEntries.Any(entry => entry.Message == message.Value)) {
                        notificationEntries.Add(new NotifyEntry { Type = NotifyType.Information, Message = message.Value });
                    }
                }
            }

            if (!notificationEntries.Any()) { return; }

            // Make the notifications available for the rest of the current request.
            filterContext.HttpContext.Items[TempDataNotifications] = notificationEntries.ToArray();
        }

        #endregion IActionFilter Members

        #region IResultFilter Members

        /// <inheritdoc/>
        public void OnResultExecuted(ResultExecutedContext filterContext) { }

        /// <inheritdoc/>
        public void OnResultExecuting(ResultExecutingContext filterContext) {
            if (!(filterContext.Result is ViewResultBase)) {
                return;
            }

            var messageEntries = filterContext
                .HttpContext
                .Items[TempDataNotifications] as IEnumerable<NotifyEntry> ?? Enumerable.Empty<NotifyEntry>();

            ((ViewResultBase)filterContext.Result)
                .TempData[TempDataNotifications] = messageEntries;
        }

        #endregion IResultFilter Members
    }
}