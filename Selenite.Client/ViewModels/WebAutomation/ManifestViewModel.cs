using System.Collections.ObjectModel;
using Selenite.Models;

namespace Selenite.Client.ViewModels.WebAutomation
{
    public class ManifestViewModel : ViewModelBase
    {
        public ManifestViewModel()
        {
            TestCollections = new ObservableCollection<TestCollection>();
        }

        public string Name
        {
            get { return Get(() => Name); }
            set { Set(value, () => Name); }
        }

        public string DefaultDomain
        {
            get { return Get(() => DefaultDomain); }
            set { Set(value, () => DefaultDomain); }
        }

        public ObservableCollection<TestCollection> TestCollections { get; set; }
    }
}