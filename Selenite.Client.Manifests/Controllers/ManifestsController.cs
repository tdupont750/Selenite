using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using Common.Constants;
using Common.Events;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using Microsoft.Win32;
using Selenite.Client.Manifests.ViewModels;
using Selenite.Client.Manifests.Views;
using Selenite.Models;
using Selenite.Services;

namespace Selenite.Client.Manifests.Controllers
{
    public class ManifestsController : IManifestsController
    {
        private readonly IUnityContainer _container;
        private readonly IRegionManager _regionManager;
        private readonly IEventAggregator _eventAggregator;
        private readonly IConfigurationService _configurationService;
        private readonly IManifestService _manifestService;
        private readonly ITestCollectionService _testCollectionService;

        public ManifestsController(
            IUnityContainer container,
            IRegionManager regionManager,
            IEventAggregator eventAggregator,
            IConfigurationService configurationService,
            IManifestService manifestService,
            ITestCollectionService testCollectionService)
        {
            _container = container;
            _regionManager = regionManager;
            _eventAggregator = eventAggregator;
            _configurationService = configurationService;
            _manifestService = manifestService;
            _testCollectionService = testCollectionService;
        }

        public void Initialize()
        {
            var viewModel = GetManifestsViewModel();
            _container.RegisterInstance(viewModel);

            var region = _regionManager.Regions[RegionNames.Navigation];
            var view = _container.Resolve<ManifestsView>();

            region.Add(view);
        }

        private ManifestsViewModel GetManifestsViewModel()
        {
            var manifestsModel = new ManifestsViewModel();
            manifestsModel.EditTestCollectionCommand = new DelegateCommand(
                        () => _eventAggregator.GetEvent<EditTestCollectionEvent>().Publish(manifestsModel.SelectedManifest.Name));

            manifestsModel.LoadManifestCommand = new DelegateCommand(() =>
                {
                    var dialog = new OpenFileDialog
                        {
                            Filter = "Manifest File|.manifests.json",
                        };

                    var result = dialog.ShowDialog();
                    if (result == true)
                    {
                        _configurationService.TestScriptsPath = Path.GetDirectoryName(dialog.FileName);
                        _manifestService.ReloadManifest();
                        LoadManifests(manifestsModel);
                    }
                });

            manifestsModel.SelectedManifestChangedCommand = new DelegateCommand<ManifestViewModel>(
                selectedManifest => _manifestService.SetActiveManifest(selectedManifest.Name));

            LoadManifests(manifestsModel);

            return manifestsModel;
        }

        private void LoadManifests(ManifestsViewModel manifestsModel)
        {
            manifestsModel.Manifests.Clear();

            try
            {
                var manifests = _manifestService.GetManifestNames();

                foreach (var manifestName in manifests)
                {
                    var manifest = _manifestService.GetManifest(manifestName);

                    manifestsModel.Manifests.Add(new ManifestViewModel
                        {
                            Name = manifestName,
                            Description = manifest.Description,
                            DomainOverride = manifest.OverrideDomain,
                            TestCollections = new ObservableCollection<TestCollectionViewModel>(LoadTestCollections(manifestName)),
                            DomainOverrideChangedCommand = 
                                new DelegateCommand<string>(
                                    param => _manifestService.SetActiveManifestDomain(param != null ? param.ToString() : string.Empty))
                        });
                }

                var selectedManifestName = _manifestService.GetActiveManifestName();
                manifestsModel.SelectedManifest = manifestsModel.Manifests.FirstOrDefault(m => m.Name == selectedManifestName);
            }
            catch (Exception e)
            {
                // TODO: Get rid of the messagebox, it's a nasty blocking call.
                MessageBox.Show(e.Message, "Error Loading Manifest List", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private IEnumerable<TestCollectionViewModel> LoadTestCollections(string manifestName)
        {
            var testCollections = new List<TestCollectionViewModel>();

            var manifest = _manifestService.GetManifest(manifestName);

            foreach (var file in manifest.Files)
            {
                TestCollection testCollection;
                try
                {
                    testCollection = _testCollectionService.GetTestCollection(file);
                }
                catch (Exception e)
                {
                    MessageBox.Show("TestCollection: " + file + Environment.NewLine + e,
                                    "Error Loading TestCollection", MessageBoxButton.OK, MessageBoxImage.Error);
                    throw;
                }

                testCollections.Add(new TestCollectionViewModel
                    {
                        Name = testCollection.File,
                        IsEnabled = testCollection.Enabled,
                        IsEnabledChangedCommand = new DelegateCommand<bool?>(enabled =>
                        {
                            testCollection.Enabled = enabled.GetValueOrDefault();
                            _testCollectionService.SaveTestCollectionInfo(testCollection);
                        })
                    });
            }

            return testCollections;
        }
    }
}
