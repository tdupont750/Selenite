using System.Windows.Input;

namespace Selenite.Client.ViewModels.WebAutomation
{
    public class TestCollectionSummaryViewModel : ViewModelBase
    {
        public string Name { get; set; }

        public bool IsEnabled
        {
            get { return Get(() => IsEnabled); }
            set { Set(value, () => IsEnabled); }
        }

        public ICommand IsEnabledChangedCommand { get; set; }
    }
}
