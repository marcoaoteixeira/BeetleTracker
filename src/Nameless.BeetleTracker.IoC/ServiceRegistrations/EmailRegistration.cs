using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac;
using Nameless.BeetleTracker.Email;

namespace Nameless.BeetleTracker.IoC.ServiceRegistrations {

    /// <summary>
    /// Autofac module implementation for Nameless.BeetleTracker.Email namespace.
    /// </summary>
    public class EmailRegistration : ServiceRegistrationBase {

        #region Public Properties

        /// <summary>
        /// Gets or sets the <see cref="IEmailService"/> implementation.
        /// </summary>
        /// <remarks>Default is <see cref="EmailService"/>.</remarks>
        public Type EmailServiceImplementation { get; set; } = typeof(EmailService);

        /// <summary>
        /// Gets or sets the <see cref="IEmailService"/><see cref="LifetimeScopeType"/>.
        /// </summary>
        /// <remarks>Default is <see cref="LifetimeScopeType.Singleton"/>.</remarks>
        public LifetimeScopeType EmailServiceLifetimeScope { get; set; } = LifetimeScopeType.Singleton;

        #endregion Public Properties

        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of <see cref="EmailRegistration"/>.
        /// </summary>
        public EmailRegistration()
            : base(null) { }

        /// <summary>
        /// Initializes a new instance of <see cref="EmailRegistration"/>.
        /// </summary>
        /// <param name="supportAssemblies">The support assemblies.</param>
        public EmailRegistration(IEnumerable<Assembly> supportAssemblies)
            : base(supportAssemblies) { }

        #endregion Public Constructors

        #region Public Override Methods

        /// <inheritdoc/>
        public override void Register() {
            Builder
                .RegisterType(GetEmailServiceImplementation())
                .As<IEmailService>()
                .SetLifetimeScope(EmailServiceLifetimeScope);
        }

        #endregion Public Override Methods

        #region Private Methods

        private Type GetEmailServiceImplementation() {
            return EmailServiceImplementation == null
                ? GetImplementationsFromSupportAssemblies(typeof(IEmailService)).SingleOrDefault()
                : EmailServiceImplementation;
        }

        #endregion Private Methods
    }
}