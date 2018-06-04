using System.IO;
using Autofac;
using Nameless.BeetleTracker.Environment;
using Nameless.BeetleTracker.Search;

namespace Nameless.BeetleTracker.IoC.ServiceRegistrations {

    /// <summary>
    /// Search service registration.
    /// </summary>
    public sealed class SearchServiceRegistration : ServiceRegistrationBase {

        // TODO: AJUSTAR ESTE REGISTRATION

        #region Private Methods

        private LuceneSettings GetLuceneSettings(IComponentContext ctx) {
            var hostingEnvironment = ctx.Resolve<IHostingEnvironment>();
            var dataDirectory = (string)hostingEnvironment.GetData("DATADIRECTORY");

            return new LuceneSettings {
                IndexStorageDirectoryPath = Path.Combine(dataDirectory, "LuceneStorage")
            };
        }

        #endregion Private Methods

        #region Public Override Methods

        /// <inheritdoc/>
        public override void Register() {
            Builder
                .Register(GetLuceneSettings)
                .SingleInstance();
            Builder
                .RegisterType<IndexProvider>()
                .As<IIndexProvider>()
                .SingleInstance();
            Builder
                .RegisterType<AnalyzerProvider>()
                .As<IAnalyzerProvider>()
                .SingleInstance();
            Builder
                .RegisterType<AnalyzerSelector>()
                .As<IAnalyzerSelector>()
                .SingleInstance();
            Builder
                .RegisterType<SearchBuilder>()
                .As<ISearchBuilder>()
                .InstancePerDependency();
        }

        #endregion Public Override Methods
    }
}