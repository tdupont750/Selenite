namespace Selenite.Client.V2
{
    /// <summary>
    /// Interaction logic for HelpShell.xaml
    /// </summary>
    public partial class HelpShell
    {
        public HelpShell()
        {
            InitializeComponent();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // Cancel the close and hide instead so our reference stays valid.
            e.Cancel = true;
            Hide();
        }
    }
}
