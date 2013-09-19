using System.Collections.ObjectModel;
using Common.ViewModels;
using Selenite.Commands;

namespace Selenite.Client.TestCollections.ViewModels
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