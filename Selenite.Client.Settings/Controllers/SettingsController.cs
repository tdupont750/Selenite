using System;
using System.Linq;
using Common.Constants;
using Common.Events;
using Common.Models;
using Common.Services;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using Selenite.Client.Settings.ViewModels;
using Selenite.Client.Settings.Views;

namespace Selenite.Client.Settings.Controllers
{
    public class SettingsController : ISettingsController
    {
        private readonly IUnityContainer _container;
        private readonly IRegionManager _regionManager;
        private readonly IEventAggregator _eventAggregator;
        private readonly ISettingsService _settingsService;

        private SettingsViewModel _viewModel;

        public SettingsController(IUnityContainer container, IRegionManager regionManager, IEventAggregator eventAggregator, ISettingsService settingsService)
        {
            _container = container;
            _regionManager = regionManager;
            _eventAggregator = eventAggregator;
            _settingsService = settingsService;
        }

        public void Initialize()
        {
            _settingsService.UpdateTheme();

            _viewModel = new SettingsViewModel
                {
                    Accents = Enum.GetValues(typeof(Accent)).Cast<Accent>().ToList(),
                    SelectedAccent = _settingsService.GetAccent(),
                    Themes = Enum.GetValues(typeof(Theme)).Cast<Theme>().ToList(),
                    SelectedTheme = _settingsService.GetTheme(),

                    IsDirty = false,

                    CloseWindowCommand = new DelegateCommand(CloseSettings),
                    SelectionChangedCommand = new DelegateCommand(CheckDirtyState),
                };

            _viewModel.ApplyChangesCommand = new DelegateCommand(ApplySettings, () => _viewModel.IsDirty);
            _viewModel.SaveChangesCommand = new DelegateCommand(SaveSettings, () => _viewModel.IsDirty);

            _container.RegisterInstance(_viewModel);

            var region = _regionManager.Regions[RegionNames.Settings];
            var view = _container.Resolve<SettingsView>();

            region.Add(view);
        }

        private void CheckDirtyState()
        {
            _viewModel.IsDirty = _viewModel.SelectedTheme != _settingsService.GetTheme()
                      || _viewModel.SelectedAccent != _settingsService.GetAccent();
        }

        private void ApplySettings()
        {
            _settingsService.SetAccent(_viewModel.SelectedAccent);
            _settingsService.SetTheme(_viewModel.SelectedTheme);
            _settingsService.UpdateTheme();

            _viewModel.IsDirty = false;
        }

        private void SaveSettings()
        {
            ApplySettings();
            CloseSettings();
        }

        private void CloseSettings()
        {
            _eventAggregator.GetEvent<HideSettingsEvent>().Publish(true);
        }
    }
}
