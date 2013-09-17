using System.Collections.ObjectModel;
using System.Windows.Input;
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

                if (DomainOverrideChangedCommand != null && DomainOverrideChangedCommand.CanExecute(null))
                {
                    DomainOverrideChangedCommand.Execute(null);
                }
            }
        }

        public ObservableCollection<TestCollectionViewModel> TestCollections { get; set; }

        public ICommand DomainOverrideChangedCommand { get; set; }
    }
}
