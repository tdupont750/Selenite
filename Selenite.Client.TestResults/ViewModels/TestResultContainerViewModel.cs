using System.ComponentModel;
using Common.ViewModels;

namespace Selenite.Client.TestResults.ViewModels
{
    public class TestResultContainerViewModel : ViewModelBase
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollectionView TestResults { get; set; }
    }
}
