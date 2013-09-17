using System.Windows.Controls;
using Selenite.Client.Manifests.ViewModels;

namespace Selenite.Client.Manifests.Views
{
    /// <summary>
    /// Interaction logic for ManifestsView.xaml
    /// </summary>
    public partial class ManifestsView
    {
        public ManifestsView(ManifestsViewModel viewModel)
        {
            InitializeComponent();

            DataContext = viewModel;
        }
    }
}
