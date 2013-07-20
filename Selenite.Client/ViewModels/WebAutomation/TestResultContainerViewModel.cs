using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;

namespace Selenite.Client.ViewModels.WebAutomation
{
    public class TestResultContainerViewModel
    {
        public string Name { get; set; }
        public ICollectionView TestResults { get; set; }
    }
}
