using System.Collections.ObjectModel;

namespace Selenite.Client.ViewModels.WebAutomation
{
    public class TestResultCollectionViewModel : ViewModelBase
    {
        public TestResultCollectionViewModel()
        {
            TestContainers = new ObservableCollection<TestResultContainerViewModel>();
        }

        public string Name { get; set; }
        public ObservableCollection<TestResultContainerViewModel> TestContainers { get; set; }
    }
}
