using System.Collections.ObjectModel;
using Common.ViewModels;

namespace Selenite.Client.TestResults.ViewModels
{
    public class TestResultCollectionViewModel : ViewModelBase
    {
        public TestResultCollectionViewModel()
        {
            TestContainers = new ObservableCollection<TestResultContainerViewModel>();
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public ObservableCollection<TestResultContainerViewModel> TestContainers { get; set; }
    }
}
