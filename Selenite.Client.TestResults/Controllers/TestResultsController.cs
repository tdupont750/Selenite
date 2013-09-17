using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using Selenite.Client.TestResults.ViewModels;
using Selenite.Client.TestResults.Views;

namespace Selenite.Client.TestResults.Controllers
{
    public class TestResultsController : ITestResultsController
    {
        private readonly IUnityContainer _container;
        private readonly IRegionManager _regionManager;

        public TestResultsController(IUnityContainer container, IRegionManager regionManager)
        {
            _container = container;
            _regionManager = regionManager;
        }

        public void Initialize()
        {
            var viewModel = new TestResultsViewModel();
            _container.RegisterInstance(viewModel);

            _regionManager.RegisterViewWithRegion(Common.Constants.RegionNames.MainContent, typeof(TestResultsView));
        }
    }
}
