using System.Windows;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Selenite.Services;
using Selenite.Services.Implementation;

namespace Selenite.Client
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        /// <summary>
        /// Gets the service locator.
        /// </summary>
        public static IServiceLocator ServiceLocator { get; private set; }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Application.Startup"/> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.StartupEventArgs"/> that contains the event data.</param>
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var unityContainer = new UnityContainer();

            unityContainer.RegisterType<IFileService, FileService>();
            unityContainer.RegisterType<IConfigurationService, ConfigurationService>();
            unityContainer.RegisterType<ICommandService, CommandService>();
            unityContainer.RegisterType<ICategoryService, CategoryService>();
            unityContainer.RegisterType<IManifestService, ManifestService>();

            ServiceLocator = new UnityServiceLocator(unityContainer);
        }
    }
}