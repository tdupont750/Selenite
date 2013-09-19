using System.Linq;
using System.Windows.Data;
using Common.Constants;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using Selenite.Client.CommandHelp.ViewModels;
using Selenite.Client.CommandHelp.Views;
using Selenite.Services;

namespace Selenite.Client.CommandHelp.Controllers
{
    public class CommandHelpController : ICommandHelpController
    {
        private readonly IUnityContainer _container;
        private readonly IRegionManager _regionManager;
        private readonly ICommandService _commandService;

        public CommandHelpController(IUnityContainer container, IRegionManager regionManager, ICommandService commandService)
        {
            _container = container;
            _regionManager = regionManager;
            _commandService = commandService;
        }

        public void Initialize()
        {
            var commands = _commandService.GetCommandNames()
                .Where(command => !command.Item1.Contains("Base"))
                .OrderBy(command => command.Item1)
                .Select(command => new CommandViewModel
                    {
                        Name = command.Item1,
                        Description = command.Item2,
                        Properties = _commandService.GetCommandProperties(command.Item1)
                                       .Where(prop => prop.Item1 != "Test")
                                       .Select(prop => new PropertyViewModel
                                           {
                                               Name = prop.Item1,
                                               Description = prop.Item2,
                                           })
                                       .ToList(),
                    })
                .ToList();

            var viewModel = new CommandHelpViewModel
                {
                    Commands = CollectionViewSource.GetDefaultView(commands),
                };

            viewModel.FilterCommandsCommand = new DelegateCommand<string>(filter =>
                {
                    _commandFilter = filter;
                    viewModel.Commands.Filter = FilterCommands;
                });

            _container.RegisterInstance(viewModel);

            var region = _regionManager.Regions[RegionNames.HelpContent];
            var view = _container.Resolve<CommandHelpView>();

            region.Add(view);
        }

        private string _commandFilter = string.Empty;

        private bool FilterCommands(object source)
        {
            var command = source as CommandViewModel;

            return command != null && command.Name.ToLower().Contains(_commandFilter.ToLower());
        }

    }
}
