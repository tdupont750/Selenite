using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Unity;
using Selenite.Client.TestResults.Controllers;

namespace Selenite.Client.TestResults
{
    public class TestResultsModule : IModule
    {
        private readonly IUnityContainer _container;

        public TestResultsModule(IUnityContainer container)
        {
            _container = container;
        }

        /// <summary>
        /// Notifies the module that it has be initialized.
        /// </summary>
        public void Initialize()
        {
            _container.RegisterType<ITestResultsController, TestResultsController>();
            _container.Resolve<ITestResultsController>().Initialize();
        }
    }
}
