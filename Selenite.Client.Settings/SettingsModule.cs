using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Unity;
using Selenite.Client.Settings.Controllers;

namespace Selenite.Client.Settings
{
    public class SettingsModule : IModule
    {
        private readonly IUnityContainer _container;

        public SettingsModule(IUnityContainer container)
        {
            _container = container;
        }

        /// <summary>
        /// Notifies the module that it has be initialized.
        /// </summary>
        public void Initialize()
        {
            _container.RegisterType<ISettingsController, SettingsController>(new ContainerControlledLifetimeManager());
            _container.Resolve<ISettingsController>().Initialize();
        }
    }
}
