using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using Selenite.Models;
using Selenite.Services;

namespace Selenite.Client.ViewModels.WebAutomation
{
    public class EditCategoryViewModel : ViewModelBase
    {
        private readonly ICommandService _commandService;
        private readonly ICategoryService _categoryService;

        public EditCategoryViewModel(ICommandService commandService, ICategoryService categoryService)
        {
            _commandService = commandService;
            _categoryService = categoryService;

            Categories = new ObservableCollection<CategoryViewModel>();

            LoadCategories();

            SaveCommand = new RelayCommand(t =>
                {
                    SaveCategories();

                    if(CancelCommand != null)
                        CancelCommand.Execute(null);
                });
        }

        public ObservableCollection<CategoryViewModel> Categories { get; set; }

        public ICommand SaveCommand { get; set; }

        public ICommand CancelCommand { get; set; }

        private void LoadCategories()
        {
            IList<string> categoryNames = null;

            try
            {
                categoryNames = _categoryService.GetCategoryNames();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error Loading Categories", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            if (categoryNames == null)
                return;

            foreach (var categoryName in categoryNames)
            {
                Category category = null;

                try
                {
                    category = _categoryService.GetCategory(categoryName);
                }
                catch (Exception e)
                {
                    MessageBox.Show("Category: " + categoryName + Environment.NewLine + e.Message,
                                    "Error Loading Category", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                if (category == null)
                    return;

                var categoryViewModel = new CategoryViewModel
                    {
                        Domain = category.Domain,
                        IsEnabled = category.Enabled,
                        Name = category.Name
                    };

                if (category.Tests == null)
                    return;

                foreach (var test in category.Tests)
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

                    categoryViewModel.Children.Add(testViewModel);
                }

                Categories.Add(categoryViewModel);
            }
        }

        private void SaveCategories()
        {

        }
    }
}