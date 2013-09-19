using System.Linq;
using Common.Events;
using Common.Services;
using MahApps.Metro;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Prism.UnityExtensions;
using System.Windows;
using Microsoft.Practices.Unity;
using Selenite.Client.CommandHelp;
using Selenite.Client.Manifests;
using Selenite.Client.TestCollections;
using Selenite.Client.TestResults;
using Selenite.Services;
using Selenite.Services.Implementation;

namespace Selenite.Client.V2
{
    public class Bootstrapper : UnityBootstrapper
    {
        private HelpShell _helpShell;
        /// <summary>
        /// Creates the shell or main window of the application.
        /// </summary>
        /// <returns>
        /// The shell of the application.
        /// </returns>
        /// <remarks>
        /// If the returned instance is a <see cref="T:System.Windows.DependencyObject"/>, the
        ///             <see cref="T:Microsoft.Practices.Prism.Bootstrapper"/> will attach the default <seealso cref="T:Microsoft.Practices.Prism.Regions.IRegionManager"/> of
        ///             the application in its <see cref="F:Microsoft.Practices.Prism.Regions.RegionManager.RegionManagerProperty"/> attached property
        ///             in order to be able to add regions by using the <seealso cref="F:Microsoft.Practices.Prism.Regions.RegionManager.RegionNameProperty"/>
        ///             attached property from XAML.
        /// </remarks>
        protected override DependencyObject CreateShell()
        {
            var shell = Container.TryResolve<Shell>();
            shell.Show();

            RegisterServices();
            RegisterEvents();

            _helpShell = Container.TryResolve<HelpShell>();
            RegionManager.SetRegionManager(_helpShell, Container.TryResolve<IRegionManager>());

            return shell;
        }

        protected override IModuleCatalog CreateModuleCatalog()
        {
            var catalog = new ModuleCatalog();

            catalog.AddModule(typeof(ManifestsModule));
            catalog.AddModule(typeof(TestResultsModule));
            catalog.AddModule(typeof(TestCollectionsModule));
            catalog.AddModule(typeof(CommandHelpModule));

            return catalog;
        }

        private void RegisterEvents()
        {
            var eventAggregator = Container.TryResolve<IEventAggregator>();
            eventAggregator.GetEvent<ShowHelpWindowEvent>().Subscribe(OnShowHelp, ThreadOption.UIThread);
        }

        private void OnShowHelp(string args)
        {
            _helpShell.Show();
        }

        private void RegisterServices()
        {
            Container.RegisterType<IEventAggregator, EventAggregator>(new ContainerControlledLifetimeManager());
            Container.RegisterType<ISettingsService, SettingsService>(new ContainerControlledLifetimeManager());
            Container.RegisterType<IFileService, FileService>(new ContainerControlledLifetimeManager());
            Container.RegisterType<ICommandService, CommandService>(new ContainerControlledLifetimeManager());
            Container.RegisterType<IManifestService, ManifestService>(new ContainerControlledLifetimeManager());
            Container.RegisterType<ITestCollectionService, TestCollectionService>(new ContainerControlledLifetimeManager());
            Container.RegisterType<IConfigurationService, ConfigurationService>(new ContainerControlledLifetimeManager());
        }
    }
}
