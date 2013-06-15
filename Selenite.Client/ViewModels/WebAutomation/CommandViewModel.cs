using System.Collections.ObjectModel;
using Selenite.Commands;

namespace Selenite.Client.ViewModels.WebAutomation
{
    public class CommandViewModel : ViewModelBase
    {
        public CommandViewModel()
        {
            Properties = new ObservableCollection<CommandPropertyViewModel>();
        }

        public string Name
        {
            get { return Get(() => Name); }
            set { Set(value, () => Name); }
        }

        public ICommand Command { get; set; }

        public ObservableCollection<CommandPropertyViewModel> Properties { get; set; }
    }
}