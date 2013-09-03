using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using System.Windows.Input;
using Selenite.Global;
using Selenite.Services;

namespace Selenite.Client.ViewModels.WebAutomation.Help
{
    public class HelpWindowViewModel : ViewModelBase
    {
        private ICommandService _commandService;

        public HelpWindowViewModel()
        {
            _commandService = ServiceResolver.Get<ICommandService>();

            var commands = _commandService.GetCommandNames()
                .Where(command => !command.Item1.Contains("Base"))
                .OrderBy(command => command.Item1)
                .Select(command => new CommandHelpViewModel
                    {
                        Name = command.Item1,
                        Description = command.Item2,
                        Properties = _commandService.GetCommandProperties(command.Item1)
                                       .Where(prop => prop.Item1 != "Test")
                                       .Select(prop => new CommandPropertyHelpViewModel
                                           {
                                               Name = prop.Item1,
                                               Description = prop.Item2,
                                           })
                                       .ToList(),
                    })
                .ToList();

            Commands = CollectionViewSource.GetDefaultView(commands);
            ClearFilterCommand = new RelayCommand(parameter => CommandFilter = string.Empty);
        }

        public ICommand ClearFilterCommand { get; set; }

        private bool FilterCommands(object source)
        {
            var command = source as CommandHelpViewModel;

            return command != null && command.Name.ToLower().Contains(CommandFilter.ToLower());
        }

        public string CommandFilter
        {
            get { return Get(() => CommandFilter); }
            set
            {
                Set(value, () => CommandFilter);
                Commands.Filter = FilterCommands;
            }
        }

        public ICollectionView Commands { get; set; }
    }
}
