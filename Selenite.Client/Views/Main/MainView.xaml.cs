using Selenite.Client.ViewModels.Main;

namespace Selenite.Client.Views.Main
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView
    {
        public MainView()
        {
            InitializeComponent();

            DataContext = new MainViewModel();
        }
    }
}