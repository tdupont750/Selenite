using System.Collections.Generic;
using Selenite.Commands;

namespace Selenite.Client.ViewModels.WebAutomation
{
    public class CommandViewModel : ViewModelBase
    {
        public string Name
        {
            get { return Get(() => Name); }
            set { Set(value, () => Name); }
        }

        public ICommand Command { get; set; }

        public IDictionary<string, string> Properties { get; set; }
    }
}