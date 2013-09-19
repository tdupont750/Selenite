using Common.Constants;
using Common.Events;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using Selenite.Client.TestCollections.Views;

namespace Selenite.Client.TestCollections.Controllers
{
    public class TestCollectionsController : ITestCollectionsController
    {
        private readonly IUnityContainer _container;
        private readonly IRegionManager _regionManager;
        private readonly IEventAggregator _eventAggregator;

        public TestCollectionsController(IUnityContainer container, IRegionManager regionManager, IEventAggregator eventAggregator)
        {
            _container = container;
            _regionManager = regionManager;
            _eventAggregator = eventAggregator;
        }

        public void Initialize()
        {
            var region = _regionManager.Regions[RegionNames.MainContent];
            var view = _container.Resolve<TestCollectionsView>();

            region.Add(view);

            _eventAggregator.GetEvent<EditTestCollectionEvent>().Subscribe(arg => region.Activate(view), ThreadOption.UIThread, true);
        }
    }
}
