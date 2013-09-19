using Common.ViewModels;
using System.Collections.Generic;

namespace Selenite.Client.CommandHelp.ViewModels
{
    public class CommandViewModel : ViewModelBase
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public IList<PropertyViewModel> Properties { get; set; }
    }
}
