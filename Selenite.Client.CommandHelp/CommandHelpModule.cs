using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Unity;
using Selenite.Client.CommandHelp.Controllers;

namespace Selenite.Client.CommandHelp
{
    public class CommandHelpModule : IModule
    {
        private readonly IUnityContainer _container;

        public CommandHelpModule(IUnityContainer container)
        {
            _container = container;
        }

        /// <summary>
        /// Notifies the module that it has be initialized.
        /// </summary>
        public void Initialize()
        {
            _container.RegisterType<ICommandHelpController, CommandHelpController>();
            _container.Resolve<ICommandHelpController>().Initialize();
        }
    }
}
