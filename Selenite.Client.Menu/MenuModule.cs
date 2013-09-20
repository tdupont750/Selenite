using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Unity;
using Selenite.Client.Menu.Controllers;

namespace Selenite.Client.Menu
{
    public class MenuModule : IModule
    {
        private readonly IUnityContainer _container;

        public MenuModule(IUnityContainer container)
        {
            _container = container;
        }

        /// <summary>
        /// Notifies the module that it has be initialized.
        /// </summary>
        public void Initialize()
        {
            // Should already be registered for the app bar.
            _container.Resolve<IMenuController>().Initialize();
        }
    }
}
