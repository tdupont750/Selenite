using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Selenite.Models;
using Selenite.Services;

namespace Selenite.Client.ViewModels.WebAutomation
{
    public class EditTestCollectionViewModel : ViewModelBase
    {
        private readonly ICommandService _commandService;
        private readonly ITestCollectionService _testCollectionServiceService;

        public EditTestCollectionViewModel(ICommandService commandService, ITestCollectionService testCollectionServiceService)
        {
            _commandService = commandService;
            _testCollectionServiceService = testCollectionServiceService;

            TestCollections = new ObservableCollection<TestCollectionViewModel>();

            LoadTestCollections();

            SaveCommand = new RelayCommand(t =>
                {
                    SaveTestCollection();

                    if(CancelCommand != null)
                        CancelCommand.Execute(null);
                });

            SelectedItem = TestCollections.FirstOrDefault();
        }

        public ObservableCollection<TestCollectionViewModel> TestCollections { get; set; }

        public object SelectedItem
        {
            get { return Get(() => SelectedItem); }
            set { Set(value, () => SelectedItem); }
        }

        public ICommand SaveCommand { get; set; }

        public ICommand CancelCommand { get; set; }

        private void LoadTestCollections()
        {
            IList<string> testCollectionFiles = null;

            try
            {
                testCollectionFiles = _testCollectionServiceService.GetTestCollectionFiles();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error Loading TestCollections", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            if (testCollectionFiles == null)
                return;

            foreach (var testCollectionFile in testCollectionFiles)
            {
                TestCollection testCollection = null;

                try
                {
                    testCollection = _testCollectionServiceService.GetTestCollection(testCollectionFile);
                }
                catch (Exception e)
                {
                    MessageBox.Show("TestCollection: " + testCollectionFile + Environment.NewLine + e.Message,
                                    "Error Loading TestCollection", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                if (testCollection == null)
                    return;

                var testCollectionViewModel = new TestCollectionViewModel
                    {
                        Domain = testCollection.DefaultDomain,
                        IsEnabled = testCollection.Enabled,
                        Name = testCollection.File
                    };

                if (testCollection.Tests == null)
                    return;

                foreach (var test in testCollection.Tests)
                {
                    var testViewModel = new TestViewModel
                        {
                            IsEnabled = test.Enabled,
                            Name = test.Name,
                            Url = test.Url,
                            IsEnabledChangedCommand = new RelayCommand(enabled =>
                                {
                                    test.Enabled = (bool)enabled;
                                    _testCollectionServiceService.SaveTestCollection(testCollection);
                                })
                        };

                    if (test.Commands == null)
                        return;

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

                TestCollections.Add(testCollectionViewModel);
            }
        }

        private void SaveTestCollection()
        {

        }
    }
}