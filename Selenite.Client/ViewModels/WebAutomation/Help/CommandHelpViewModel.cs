using System.Collections.Generic;

namespace Selenite.Client.ViewModels.WebAutomation.Help
{
    public class CommandHelpViewModel : ViewModelBase
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public IList<CommandPropertyHelpViewModel> Properties { get; set; }
    }
}
