using System.ComponentModel;

namespace Selenite.Client.ViewModels.WebAutomation
{
    public class TestResultContainerViewModel : ViewModelBase
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollectionView TestResults { get; set; }
    }
}
