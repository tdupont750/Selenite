using System.Collections.ObjectModel;
using System.Windows.Input;
using Common.Extensions;
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
            set
            {
                Set(value, () => SelectedManifest);
                SelectedManifestChangedCommand.VerifyAndExecute(value);
            }
        }

        public bool AllowEdit
        {
            get { return Get(() => AllowEdit); }
            set
            {
                Set(value, () => AllowEdit);
                EditTestCollectionCommand.RaiseCanExecuteChanged();
            }
        }

        public bool TestsRunning
        {
            get { return Get(() => TestsRunning); }
            set
            {
                Set(value, () => TestsRunning);
                LoadManifestCommand.RaiseCanExecuteChanged();
                EditTestCollectionCommand.RaiseCanExecuteChanged();
            }
        }

        public ICommand LoadManifestCommand { get; set; }
        public ICommand SelectedManifestChangedCommand { get; set; }
        public ICommand EditTestCollectionCommand { get; set; }
    }
}
