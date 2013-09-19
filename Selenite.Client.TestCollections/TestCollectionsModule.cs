using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Unity;
using Selenite.Client.TestCollections.Controllers;

namespace Selenite.Client.TestCollections
{
    public class TestCollectionsModule : IModule
    {
        private readonly IUnityContainer _container;

        public TestCollectionsModule(IUnityContainer container)
        {
            _container = container;
        }

        /// <summary>
        /// Notifies the module that it has be initialized.
        /// </summary>
        public void Initialize()
        {
            _container.RegisterType<ITestCollectionsController, TestCollectionsController>();
            _container.Resolve<ITestCollectionsController>().Initialize();
        }
    }
}
