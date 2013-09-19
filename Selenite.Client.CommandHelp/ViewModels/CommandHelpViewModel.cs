using Common.Extensions;
using Common.ViewModels;
using System.ComponentModel;
using System.Windows.Input;

namespace Selenite.Client.CommandHelp.ViewModels
{
    public class CommandHelpViewModel : ViewModelBase
    {
        public string CommandFilter
        {
            get { return Get(() => CommandFilter); }
            set
            {
                Set(value, () => CommandFilter);

                FilterCommandsCommand.VerifyAndExecute(value);
            }
        }

        public ICommand FilterCommandsCommand { get; set; }
        public ICollectionView Commands { get; set; }
    }
}
