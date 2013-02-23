using System.Collections.ObjectModel;

namespace Selenite.Client.ViewModels.WebAutomation
{
    public class TestViewModel : ViewModelBase
    {
        public TestViewModel()
        {
            Assertions = new ObservableCollection<CommandViewModel>();
        }

        public string Name
        {
            get { return Get(() => Name); }
            set { Set(value, () => Name); }
        }

        public string Url
        {
            get { return Get(() => Url); }
            set { Set(value, () => Url); }
        }

        public bool IsEnabled
        {
            get { return Get(() => IsEnabled); }
            set { Set(value, () => IsEnabled); }
        }

        public ObservableCollection<CommandViewModel> Assertions { get; set; }
    }
}