using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Autofac;
using Autofac.Core;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using Castle.DynamicProxy;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataProtection;
using Nameless.BeetleTracker.Aspect;
using Nameless.BeetleTracker.Identity;
using Nameless.BeetleTracker.Identity.Services;
using Nameless.BeetleTracker.Identity.Stores;
using Nameless.BeetleTracker.IoC;
using Nameless.BeetleTracker.Security;
using Owin;

namespace Nameless.BeetleTracker {

    /// <summary>
    /// Application service registration.
    /// </summary>
    public sealed class ApplicationServiceRegistration : ServiceRegistrationBase {

        #region Private Read-Only Fields

        private readonly IAppBuilder _app;

        #endregion Private Read-Only Fields

        #region Public Properties

        /// <summary>
        /// Gets or sets the <see cref="IUserStore{TUser}"/> implementation.
        /// </summary>
        /// <remarks>Default is <see cref="UserStore{TUser}"/></remarks>
        public Type UserStoreImplementation { get; set; } = typeof(UserStore<>);

        /// <summary>
        /// Gets or sets the <see cref="IUserStore{TUser}"/><see cref="LifetimeScopeType"/>.
        /// </summary>
        /// <remarks>Default is <see cref="LifetimeScopeType.PerScope"/>.</remarks>
        public LifetimeScopeType UserStoreLifetimeScope { get; set; } = LifetimeScopeType.PerScope;

        /// <summary>
        /// Gets or sets the <see cref="IRoleStore{TRole}"/> implementation.
        /// </summary>
        /// <remarks>Default is <see cref="RoleStore{TRole}"/></remarks>
        public Type RoleStoreImplementation { get; set; } = typeof(RoleStore<>);

        /// <summary>
        /// Gets or sets the <see cref="IRoleStore{TRole}"/><see cref="LifetimeScopeType"/>.
        /// </summary>
        /// <remarks>Default is <see cref="LifetimeScopeType.PerScope"/>.</remarks>
        public LifetimeScopeType RoleStoreLifetimeScope { get; set; } = LifetimeScopeType.PerScope;

        /// <summary>
        /// Gets or sets the <see cref="UserManager{TUser}"/> implementation.
        /// </summary>
        /// <remarks>Default is <see cref="IdentityUserManager"/></remarks>
        public Type UserManagerImplementation { get; set; } = typeof(IdentityUserManager);

        /// <summary>
        /// Gets or sets the <see cref="UserManager{TUser}"/><see cref="LifetimeScopeType"/>.
        /// </summary>
        /// <remarks>Default is <see cref="LifetimeScopeType.PerScope"/>.</remarks>
        public LifetimeScopeType UserManagerLifetimeScope { get; set; } = LifetimeScopeType.PerScope;

        /// <summary>
        /// Gets or sets the <see cref="SignInManager{TUser, TKey}"/> implementation.
        /// </summary>
        /// <remarks>Default is <see cref="ApplicationSignInManager"/></remarks>
        public Type SignInManagerImplementation { get; set; } = typeof(ApplicationSignInManager);

        /// <summary>
        /// Gets or sets the <see cref="ApplicationSignInManager"/><see cref="LifetimeScopeType"/>.
        /// </summary>
        /// <remarks>Default is <see cref="LifetimeScopeType.PerScope"/>.</remarks>
        public LifetimeScopeType SignInManagerLifetimeScope { get; set; } = LifetimeScopeType.PerScope;

        /// <summary>
        /// Gets or sets the <see cref="IAuthenticationManager"/><see cref="LifetimeScopeType"/>.
        /// </summary>
        /// <remarks>Default is <see cref="LifetimeScopeType.PerScope"/>.</remarks>
        public LifetimeScopeType AuthenticationManagerLifetimeScope { get; set; } = LifetimeScopeType.PerScope;

        /// <summary>
        /// Gets or sets the <see cref="IDataProtectionProvider"/><see cref="LifetimeScopeType"/>.
        /// </summary>
        /// <remarks>Default is <see cref="LifetimeScopeType.PerScope"/>.</remarks>
        public LifetimeScopeType DataProtectionProviderLifetimeScope { get; set; } = LifetimeScopeType.PerScope;

        /// <summary>
        /// Gets or sets the <see cref="IEmailService"/> implementation.
        /// </summary>
        /// <remarks>Default is <see cref="EmailService"/></remarks>
        public Type EmailServiceImplementation { get; set; } = typeof(EmailService);

        /// <summary>
        /// Gets or sets the <see cref="IEmailService"/><see cref="LifetimeScopeType"/>.
        /// </summary>
        /// <remarks>Default is <see cref="LifetimeScopeType.PerScope"/>.</remarks>
        public LifetimeScopeType EmailServiceLifetimeScope { get; set; } = LifetimeScopeType.PerScope;

        /// <summary>
        /// Gets or sets the <see cref="ISmsService"/> implementation.
        /// </summary>
        /// <remarks>Default is <see cref="SmsService"/></remarks>
        public Type SmsServiceImplementation { get; set; } = typeof(SmsService);

        /// <summary>
        /// Gets or sets the <see cref="ISmsService"/><see cref="LifetimeScopeType"/>.
        /// </summary>
        /// <remarks>Default is <see cref="LifetimeScopeType.PerScope"/>.</remarks>
        public LifetimeScopeType SmsServiceLifetimeScope { get; set; } = LifetimeScopeType.PerScope;

        #endregion Public Properties

        #region Public Constructors

        public ApplicationServiceRegistration(IAppBuilder app)
            : this(app, supportAssemblies: null) { }

        public ApplicationServiceRegistration(IAppBuilder app, IEnumerable<Assembly> supportAssemblies)
            : base(supportAssemblies) {
            Prevent.ParameterNull(app, nameof(app));

            _app = app;
        }

        #endregion Public Constructors

        #region Public Override Methods

        /// <inheritdoc/>
        public override void Register() {
            Builder.RegisterGeneric(UserStoreImplementation).As(typeof(IUserStore<>)).SetLifetimeScope(UserStoreLifetimeScope);
            Builder.RegisterGeneric(RoleStoreImplementation).As(typeof(IRoleStore<>)).SetLifetimeScope(RoleStoreLifetimeScope);
            Builder.RegisterTypes(new[] { UserManagerImplementation }).AsClosedTypesOf(typeof(UserManager<>)).OnActivated(OnUserManagerActivated).SetLifetimeScope(UserManagerLifetimeScope);
            Builder.RegisterTypes(new[] { SignInManagerImplementation }).AsClosedTypesOf(typeof(SignInManager<,>)).SetLifetimeScope(UserManagerLifetimeScope);
            Builder.Register(ctx => HttpContext.Current.GetOwinContext().Authentication).As<IAuthenticationManager>().SetLifetimeScope(AuthenticationManagerLifetimeScope);
            Builder.Register(ctx => _app.GetDataProtectionProvider()).As<IDataProtectionProvider>().SetLifetimeScope(DataProtectionProviderLifetimeScope);
            Builder.RegisterType(EmailServiceImplementation).As<IEmailService>().As<IIdentityMessageService>().SetLifetimeScope(EmailServiceLifetimeScope);
            Builder.RegisterType(SmsServiceImplementation).As<ISmsService>().As<IIdentityMessageService>().SetLifetimeScope(SmsServiceLifetimeScope);

            Builder.RegisterControllers(SupportAssemblies);
            Builder.RegisterApiControllers(SupportAssemblies);

            Builder.Register(RegisterApplicationContext).As<IApplicationContext>().InstancePerRequest();
            Builder.RegisterSource(new ViewRegistrationSource());
            Builder.RegisterType<ApplicationContextMiddleware>().InstancePerRequest();

            // Register interceptors
            Builder
                .Register(ctx => new EventSourcingInterceptor(ctx.Resolve<IApplicationContext>()))
                .Named<IInterceptor>(typeof(EventSourcingInterceptor).FullName);

        }

        #endregion Public Override Methods

        #region Private Static Methods

        private static object RegisterApplicationContext(IComponentContext arg) {
            return arg.Resolve<IOwinContext>().Get<IApplicationContext>(ApplicationContextMiddleware.ApplicationContextKey);
        }

        private static void OnUserManagerActivated(IActivatedEventArgs<object> args) {
            var provider = args.Context.Resolve<IDataProtectionProvider>();
            ((dynamic)args.Instance).UserTokenProvider = new DataProtectorTokenProvider<IdentityUser>(provider.Create("ASP.Net Application"));
        }

        #endregion Private Static Methods
    }
}