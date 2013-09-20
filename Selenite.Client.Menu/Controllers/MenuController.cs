using Common.Events;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Unity;
using Selenite.Client.Menu.ViewModels;

namespace Selenite.Client.Menu.Controllers
{
    public class MenuController : IMenuController
    {
        private readonly IUnityContainer _container;
        private readonly IEventAggregator _eventAggregator;

        public MenuController(IUnityContainer container, IEventAggregator eventAggregator)
        {
            _container = container;
            _eventAggregator = eventAggregator;
        }

        public void InitializeAppBar()
        {
            var viewModel = new AppBarMenuViewModel
                {
                    ShowSettingsCommand = new DelegateCommand(() => _eventAggregator.GetEvent<ShowSettingsEvent>().Publish(true)),
                };

            _container.RegisterInstance(viewModel);
        }

        public void Initialize()
        {
            
        }
    }
}
