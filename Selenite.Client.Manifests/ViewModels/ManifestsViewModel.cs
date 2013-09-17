using System.Collections.ObjectModel;
using System.Windows.Input;
using Common.ViewModels;

namespace Selenite.Client.Manifests.ViewModels
{
    public class ManifestsViewModel : ViewModelBase
    {
        public ManifestsViewModel()
        {
            Manifests = new ObservableCollection<ManifestViewModel>();
        }

        public ObservableCollection<ManifestViewModel> Manifests { get; set; }
        public ManifestViewModel SelectedManifest
        {
            get { return Get(() => SelectedManifest); }
            set { Set(value, () => SelectedManifest); }
        }

        public ICommand LoadManifestCommand { get; set; }
    }
}
