using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

            LoadCategories();

            SaveCommand = new RelayCommand(t =>
                {
                    SaveCategories();

                    if(CancelCommand != null)
                        CancelCommand.Execute(null);
                });
        }

        public ObservableCollection<TestCollectionViewModel> TestCollections { get; set; }

        public ICommand SaveCommand { get; set; }

        public ICommand CancelCommand { get; set; }

        private void LoadCategories()
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
                        };

                    if (test.Commands == null)
                        return;

                    foreach (var command in test.Commands)
                    {
                        var commandViewModel = new CommandViewModel
                            {
                                Name = command.Name,
                                Command = command,
                                Properties = _commandService.GetCommandValues(command)
                            };

                        testViewModel.Children.Add(commandViewModel);
                    }

                    testCollectionViewModel.Children.Add(testViewModel);
                }

                TestCollections.Add(testCollectionViewModel);
            }
        }

        private void SaveCategories()
        {

        }
    }
}