using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Selenite.Models;
using Selenite.Services;

namespace Selenite.Client.ViewModels.WebAutomation
{
    public sealed class WebAutomationViewModel : TabbedViewModel
    {
        private readonly IManifestService _manifestService;
        private readonly ITestCollectionService _testCollectionService;

        public WebAutomationViewModel(IManifestService manifestService, ITestCollectionService testCollectionService)
        {
            _manifestService = manifestService;
            _testCollectionService = testCollectionService;

            Header = "Web Automation";

            ResultsViewModel = App.ServiceLocator.GetInstance<ResultsViewModel>();

            Manifests = new ObservableCollection<ManifestViewModel>();
            TransitionArea = ResultsViewModel;

            LoadInformation();

            EditCategoriesCommand = new RelayCommand(EditTestCollections, t => EditTestCollectionViewModel == null);
        }

        #region Properties

        public ResultsViewModel ResultsViewModel { get; set; }

        public EditTestCollectionViewModel EditTestCollectionViewModel { get; set; }

        public ViewModelBase TransitionArea
        {
            get { return Get(() => TransitionArea); }
            set { Set(value, () => TransitionArea); }
        }

        public ObservableCollection<ManifestViewModel> Manifests { get; set; }

        public ManifestViewModel SelectedManifest
        {
            get { return Get(() => SelectedManifest); }
            set
            {
                Set(value, () => SelectedManifest);
                _manifestService.SetActiveManifest(SelectedManifest.Name);
                LoadManifest(value);
            }
        }

        public ICommand EditCategoriesCommand { get; set; }

        #endregion

        private void LoadInformation()
        {
            Manifests.Clear();

            try
            {
                var manifests = _manifestService.GetManifestNames();

                foreach (var manifestName in manifests)
                {
                    var manifest = _manifestService.GetManifest(manifestName);

                    Manifests.Add(new ManifestViewModel
                        {
                            Name = manifestName,
                            DomainOverride = manifest.OverrideDomain,
                            DomainOverrideChangedCommand = new RelayCommand(param => _manifestService.SetActiveManifestDomain(param.ToString()))
                        });
                }

                var selectedManifestName = _manifestService.GetActiveManifestName();
                SelectedManifest = Manifests.FirstOrDefault(m => m.Name == selectedManifestName);
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message, "Error Loading Manifest List", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadManifest(ManifestViewModel manifestViewModel)
        {
            if (manifestViewModel == null)
                return;

            Manifest manifest = null;
            try
            {
                manifest = _manifestService.GetManifest(SelectedManifest.Name);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error Loading Manifest", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            if (manifest == null)
                return;

            manifestViewModel.TestCollections.Clear();
            SelectedManifest.DomainOverride = manifest.OverrideDomain;

            foreach (var testCollectionFile in manifest.Files)
            {
                TestCollection testCollection = null;

                try
                {
                    testCollection = _testCollectionService.GetTestCollection(testCollectionFile);
                }
                catch (Exception e)
                {
                    MessageBox.Show("TestCollection: " + testCollectionFile + Environment.NewLine + e.Message,
                                    "Error Loading TestCollection", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                if(testCollection == null)
                    continue;

                manifestViewModel.TestCollections.Add(new TestCollectionSummaryViewModel
                    {
                        IsEnabled = testCollection.Enabled,
                        Name = testCollection.File,
                        IsEnabledChangedCommand = new RelayCommand(enabled =>
                            {
                                testCollection.Enabled = (bool) enabled;
                                _testCollectionService.SaveTestCollection(testCollection);
                            })
                    });
            }
        }

        private void EditTestCollections(object parameter)
        {
            var editViewModel = App.ServiceLocator.GetInstance<EditTestCollectionViewModel>();

            editViewModel.CancelCommand = new RelayCommand(t =>
                {
                    TransitionArea = ResultsViewModel;
                    EditTestCollectionViewModel = null;
                }, t => true);

            TransitionArea = EditTestCollectionViewModel = editViewModel;
        }
    }
}