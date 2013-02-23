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
        private readonly ICategoryService _categoryService;

        public WebAutomationViewModel(IManifestService manifestService, ICategoryService categoryService)
        {
            _manifestService = manifestService;
            _categoryService = categoryService;

            Header = "Web Automation";

            ResultsViewModel = App.ServiceLocator.GetInstance<ResultsViewModel>();

            Manifests = new ObservableCollection<ManifestViewModel>();
            TransitionArea = ResultsViewModel;

            LoadInformation();

            SelectedManifest = Manifests.FirstOrDefault();

            EditCategoriesCommand = new RelayCommand(EditCategories, t => EditCategoryViewModel == null);
        }

        #region Properties

        public ResultsViewModel ResultsViewModel { get; set; }

        public EditCategoryViewModel EditCategoryViewModel { get; set; }

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

                foreach (var manifest in manifests)
                {
                    Manifests.Add(new ManifestViewModel
                        {
                            Name = manifest
                        });
                }
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

            SelectedManifest.DefaultDomain = manifest.Domain;

            foreach (var categoryName in manifest.Categories)
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

                if(category == null)
                    continue;

                manifestViewModel.Categories.Add(category);
            }
        }

        private void EditCategories(object parameter)
        {
            var editViewModel = App.ServiceLocator.GetInstance<EditCategoryViewModel>();

            editViewModel.CancelCommand = new RelayCommand(t =>
                {
                    TransitionArea = ResultsViewModel;
                    EditCategoryViewModel = null;
                }, t => true);

            TransitionArea = EditCategoryViewModel = editViewModel;
        }
    }
}