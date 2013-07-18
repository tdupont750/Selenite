using System.Collections.ObjectModel;

namespace Selenite.Client.ViewModels.WebAutomation
{
    public class TestResultContainerViewModel
    {
        public string Name { get; set; }
        public ObservableCollection<TestResultViewModel> TestResults { get; set; } 
    }
}
