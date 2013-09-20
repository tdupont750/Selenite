using System.Windows;
using Selenite.Client.Menu.ViewModels;

namespace Selenite.Client.V2
{
    /// <summary>
    /// Interaction logic for Shell.xaml
    /// </summary>
    public partial class Shell
    {
        public Shell(AppBarMenuViewModel appBarViewModel)
        {
            InitializeComponent();

            AppBarMenu.DataContext = appBarViewModel;
        }

        private void Window_Closed(object sender, System.EventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
