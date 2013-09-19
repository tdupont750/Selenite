using System.Collections.ObjectModel;
using System.Windows.Input;
using Common.ViewModels;

namespace Selenite.Client.TestCollections.ViewModels
{
    public class TestCollectionsViewModel : ViewModelBase
    {
        public TestCollectionsViewModel()
        {
            TestCollections = new ObservableCollection<TestCollectionViewModel>();
        }

        public ObservableCollection<TestCollectionViewModel> TestCollections
        {
            get { return Get(() => TestCollections); }
            set { Set(value, () => TestCollections); }
        }

        public object SelectedItem
        {
            get { return Get(() => SelectedItem); }
            set { Set(value, () => SelectedItem); }
        }

        public ICommand SaveCommand { get; set; }

        public ICommand CancelCommand { get; set; }
    }
}
