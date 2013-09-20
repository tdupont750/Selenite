
namespace Selenite.Client.V2
{
    /// <summary>
    /// Interaction logic for SettingsShell.xaml
    /// </summary>
    public partial class SettingsShell
    {
        public SettingsShell()
        {
            InitializeComponent();
        }

        private void MetroWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Hide();
            e.Cancel = true;
        }
    }
}
