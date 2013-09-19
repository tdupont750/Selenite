using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using Common.Constants;
using Common.Events;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using Selenite.Client.TestCollections.ViewModels;
using Selenite.Client.TestCollections.Views;
using Selenite.Models;
using Selenite.Services;

namespace Selenite.Client.TestCollections.Controllers
{
    public class TestCollectionsController : ITestCollectionsController
    {
        private readonly IUnityContainer _container;
        private readonly IRegionManager _regionManager;
        private readonly IEventAggregator _eventAggregator;
        private readonly ITestCollectionService _testCollectionService;
        private readonly ICommandService _commandService;

        public TestCollectionsController(IUnityContainer container, IRegionManager regionManager, IEventAggregator eventAggregator, ITestCollectionService testCollectionService, ICommandService commandService)
        {
            _container = container;
            _regionManager = regionManager;
            _eventAggregator = eventAggregator;
            _testCollectionService = testCollectionService;
            _commandService = commandService;
        }

        public void Initialize()
        {
            var viewModel = new TestCollectionsViewModel
                {
                    CancelCommand = new DelegateCommand(() => _eventAggregator.GetEvent<ShowTestResultsEvent>().Publish(string.Empty)),
                };

            _container.RegisterInstance(viewModel);

            var region = _regionManager.Regions[RegionNames.MainContent];
            var view = _container.Resolve<TestCollectionsView>();

            region.Add(view);

            _eventAggregator.GetEvent<EditTestCollectionEvent>().Subscribe(arg =>
                {
                    viewModel.TestCollections = new ObservableCollection<TestCollectionViewModel>(LoadTestCollections());
                    viewModel.SelectedItem = viewModel.TestCollections.FirstOrDefault();

                    region.Activate(view);
                }, ThreadOption.UIThread, true);

            _eventAggregator.GetEvent<SelectedManifestChangedEvent>().Subscribe(arg =>
                {
                    viewModel.TestCollections = new ObservableCollection<TestCollectionViewModel>(LoadTestCollections());
                    viewModel.SelectedItem = viewModel.TestCollections.FirstOrDefault();
                });
        }

        private IEnumerable<TestCollectionViewModel> LoadTestCollections()
        {
            var testCollections = new List<TestCollectionViewModel>();

            IList<string> testCollectionFiles = null;

            try
            {
                testCollectionFiles = _testCollectionService.GetTestCollectionFiles();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error Loading TestCollections", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            if (testCollectionFiles == null)
                return null;

            foreach (var testCollectionFile in testCollectionFiles)
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

                if (testCollection == null)
                    return null;

                var testCollectionViewModel = new TestCollectionViewModel
                {
                    Domain = testCollection.DefaultDomain,
                    IsEnabled = testCollection.Enabled,
                    Name = testCollection.File,
                    FullPath = testCollection.ResolvedFile,
                    OpenFileCommand = new DelegateCommand(() =>
                        {
                            _eventAggregator.GetEvent<ShowHelpWindowEvent>().Publish(null);

                            try
                            {
                                Process.Start(testCollection.ResolvedFile);
                            }
                            catch (Exception e)
                            {
                                // TODO: Remove blocking call.
                                MessageBox.Show(string.Format("Could not open {0}\r\n{1}", testCollection.ResolvedFile, e.Message));
                            }
                        })
                };

                if (testCollection.Tests == null)
                    return null;

                foreach (var test in testCollection.Tests)
                {
                    var testViewModel = new TestViewModel
                    {
                        IsEnabled = test.Enabled,
                        Name = test.Name,
                        Url = test.Url,
                        IsEnabledChangedCommand = new DelegateCommand<bool?>(enabled =>
                        {
                            test.Enabled = enabled.GetValueOrDefault();
                            _testCollectionService.SaveTestCollectionInfo(testCollection);
                        })
                    };

                    if (test.Commands == null)
                        return null;

                    foreach (var command in test.Commands)
                    {
                        var commandViewModel = new CommandViewModel
                        {
                            Name = command.Name,
                            Command = command,
                        };

                        var properties = _commandService.GetCommandValues(command);

                        foreach (var item in properties)
                        {
                            commandViewModel.Properties.Add(new CommandPropertyViewModel
                            {
                                Name = item.Key,
                                Value = item.Value
                            });
                        }

                        testViewModel.Children.Add(commandViewModel);
                    }

                    testCollectionViewModel.Children.Add(testViewModel);
                }

                testCollections.Add(testCollectionViewModel);
            }

            return testCollections;
        }
    }
}
