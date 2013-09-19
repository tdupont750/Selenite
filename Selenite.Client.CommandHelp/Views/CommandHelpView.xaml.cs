using Selenite.Client.CommandHelp.ViewModels;

namespace Selenite.Client.CommandHelp.Views
{
    /// <summary>
    /// Interaction logic for CommandHelpView.xaml
    /// </summary>
    public partial class CommandHelpView
    {
        public CommandHelpView(CommandHelpViewModel viewModel)
        {
            InitializeComponent();

            DataContext = viewModel;
        }
    }
}
