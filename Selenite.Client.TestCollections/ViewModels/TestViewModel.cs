using System.Windows.Input;
using Common.ViewModels;

namespace Selenite.Client.TestCollections.ViewModels
{
    public class TestViewModel : TreeViewModelBase<CommandViewModel>
    {
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

        public ICommand IsEnabledChangedCommand { get; set; }
    }
}