using Selenite.Client.ViewModels.WebAutomation.Help;

namespace Selenite.Client.Views.WebAutomation
{
    public partial class HelpWindow
    {
        public HelpWindow()
        {
            InitializeComponent();

            DataContext = new HelpWindowViewModel();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // Cancel the close and hide instead so our reference stays valid.
            e.Cancel = true;
            Hide();
        }
    }
}
