using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Unity;
using Selenite.Client.Manifests.Controllers;

namespace Selenite.Client.Manifests
{
    public class ManifestsModule : IModule
    {
        private readonly IUnityContainer _container;

        public ManifestsModule(IUnityContainer container)
        {
            _container = container;
        }

        public void Initialize()
        {
            _container.RegisterType<IManifestsController, ManifestsController>();
            _container.Resolve<IManifestsController>().Initialize();
        }
    }
}
