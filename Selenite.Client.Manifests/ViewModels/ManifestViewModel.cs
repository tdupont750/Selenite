using System.Collections.ObjectModel;
using System.Windows.Input;
using Common.Extensions;
using Common.ViewModels;

namespace Selenite.Client.Manifests.ViewModels
{
    public class ManifestViewModel : ViewModelBase
    {
        public ManifestViewModel()
        {
            TestCollections = new ObservableCollection<TestCollectionViewModel>();
        }

        public string Name { get; set; }
        public string Description { get; set; }

        public string DomainOverride
        {
            get { return Get(() => DomainOverride); }
            set
            {
                Set(value, () => DomainOverride);

                DomainOverrideChangedCommand.VerifyAndExecute(value);
            }
        }

        public ObservableCollection<TestCollectionViewModel> TestCollections { get; set; }

        public ICommand DomainOverrideChangedCommand { get; set; }
    }
}
