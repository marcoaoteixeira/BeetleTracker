using System;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Castle.DynamicProxy;
using Nameless.BeetleTracker.Data;
using Nameless.BeetleTracker.EventSourcing.Events;
using Nameless.BeetleTracker.EventSourcing.Snapshots;
using Nameless.BeetleTracker.Logging;

namespace Nameless.BeetleTracker.Aspect {

    /// <summary>
    /// Interceptor implementation for event sourcing - database integration.
    /// </summary>
    public class EventSourcingInterceptor : IInterceptor {

        #region Private Static Read-Only Fields

        private static readonly MethodInfo[] AcceptableInvocationMethods = new[] {
            typeof(IDatabase).GetMethod(nameof(IDatabase.ExecuteNonQuery)),
            typeof(IDatabase).GetMethod(nameof(IDatabase.ExecuteReader)),
            typeof(IDatabase).GetMethod(nameof(IDatabase.ExecuteScalar))
        };

        private static readonly MethodInfo[] AcceptableOriginMethods = new[] {
            typeof(IEventStore).GetMethod(nameof(IEventStore.Get)),
            typeof(IEventStore).GetMethod(nameof(IEventStore.Save)),
            typeof(ISnapshotStore).GetMethod(nameof(ISnapshotStore.Get)),
            typeof(ISnapshotStore).GetMethod(nameof(ISnapshotStore.Save))
        };

        #endregion Private Static Read-Only Fields

        #region Private Read-Only Fields

        private readonly IApplicationContext _context;

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
        /// Initializes a new instance of <see cref="EventSourcingInterceptor"/>
        /// </summary>
        /// <param name="context">The application context.</param>
        public EventSourcingInterceptor(IApplicationContext context) {
            Prevent.ParameterNull(context, nameof(context));

            _context = context;
        }

        #endregion Public Constructors

        #region IInterceptor Members

        /// <inheritdoc/>
        public void Intercept(IInvocation invocation) {
            var invokedMethod = invocation.GetConcreteMethod();
            var eventSourcingMethod = new StackFrame(3).GetMethod();
            var canIntercept = AcceptableInvocationMethods.Any(invokedMethod.Equals) &&
                AcceptableOriginMethods.Any(eventSourcingMethod.Equals);

            if (canIntercept) {
                Logger.Information($"Method intercepted by {nameof(EventSourcingInterceptor)}: {invokedMethod.DeclaringType.FullName}::{invokedMethod.Name}");

                // Gets the parameters array
                var parameters = invokedMethod.GetParameters().FirstOrDefault(_ => _.Name == "parameters");
                if (parameters != null) {
                    var parametersValues = ((Parameter[])invocation.GetArgumentValue(parameters.Position)).ToList();

                    // Creates a new parameter (for entity Owner - or - UserId) and adds it to the
                    // parameters list.
                    object newParameterValue = null;
                    if (_context.User != null && _context.User.ID != Guid.Empty) {
                        newParameterValue = _context.User.ID;
                    }
                    parametersValues.Add(Parameter.CreateInputParameter("Owner", newParameterValue, DbType.Guid));

                    // Updates the method parameter
                    invocation.SetArgumentValue(parameters.Position, parametersValues.ToArray());
                }
            }

            // Carry on the invocation.
            invocation.Proceed();
        }

        #endregion IInterceptor Members
    }
}