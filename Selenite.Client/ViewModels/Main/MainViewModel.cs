using System.Collections.ObjectModel;
using System.Linq;
using Selenite.Client.ViewModels.WebAutomation;

namespace Selenite.Client.ViewModels.Main
{
    public sealed class MainViewModel : ViewModelBase
    {
        public MainViewModel()
        {
            Title = "ITSA Tool - Now Edition";

            TabItems = new ObservableCollection<TabbedViewModel>
                {
                    App.ServiceLocator.GetInstance<WebAutomationViewModel>(),
                };

            SelectedTabItem = TabItems.FirstOrDefault();
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