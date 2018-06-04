using System.Reflection;
using System.Web.Http;
using System.Web.Mvc;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using Nameless.BeetleTracker.IoC;
using Nameless.BeetleTracker.IoC.ServiceRegistrations;
using Owin;

namespace Nameless.BeetleTracker {

    public partial class StartUp {

        #region Private Properties

        private ICompositionRoot CompositionRoot { get; } = new CompositionRoot();

        #endregion Private Properties

        #region Private Methods

        private void ConfigureCompositionRoot(IAppBuilder app, HttpConfiguration configuration) {
            var supportAssemblies = new[] {
                Assembly.Load("Nameless.BeetleTracker.Application"),
                Assembly.Load("Nameless.BeetleTracker.Core"),
                Assembly.Load("Nameless.BeetleTracker.Framework"),
                Assembly.Load("Nameless.BeetleTracker.Framework.Impl"),
                Assembly.Load("Nameless.BeetleTracker.Web")
            };

            CompositionRoot.Compose(new IServiceRegistration[] {
                new CacheServiceRegistration(),
                new DatabaseServiceRegistration(),
                new EmailRegistration(),
                new EnvironmentServiceRegistration(),
                new ErrorHandlingServiceRegistration(),
                new EventSourcingServiceRegistration(supportAssemblies),
                new LocalizationServiceRegistration(),
                new LoggingServiceRegistration(),
                new NotifierServiceRegistration(),
                new PubSubServiceRegistration(),
                new ServicesServiceRegistration(),
                new SettingsServiceRegistration(),
                new TextServiceRegistration(),

                new ApplicationServiceRegistration(app, supportAssemblies)
            });

            CompositionRoot.StartUp();

            var container = ((CompositionRoot)CompositionRoot).Container;
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
            configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);

            app.UseAutofacMiddleware(container);
            app.UseAutofacWebApi(configuration);

            app.UseAutofacMvc();
        }

        #endregion Private Methods
    }
}