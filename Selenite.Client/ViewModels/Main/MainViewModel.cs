using System.Collections.ObjectModel;
using System.Linq;
using Selenite.Client.ViewModels.WebAutomation;
using Selenite.Client.Views.WebAutomation;
using Selenite.Global;

namespace Selenite.Client.ViewModels.Main
{
    public sealed class MainViewModel : ViewModelBase
    {
        public MainViewModel()
        {
            Title = "Selenite";

            TabItems = new ObservableCollection<TabbedViewModel>
                {
                    App.ServiceLocator.GetInstance<WebAutomationViewModel>(),
                };

            SelectedTabItem = TabItems.FirstOrDefault();

            ServiceResolver.Register(new HelpWindow());
        }

        public string Title
        {
            get { return Get(() => Title); }
            set { Set(value, () => Title); }
        }

        public ObservableCollection<TabbedViewModel> TabItems { get; set; }

        public TabbedViewModel SelectedTabItem
        {
            get { return Get(() => SelectedTabItem); }
            set { Set(value, () => SelectedTabItem); }
        }
    }
}