using Selenite.Client.TestCollections.ViewModels;

namespace Selenite.Client.TestCollections.Views
{
    /// <summary>
    /// Interaction logic for TestCollectionsView.xaml
    /// </summary>
    public partial class TestCollectionsView
    {
        public TestCollectionsView(TestCollectionsViewModel viewModel)
        {
            InitializeComponent();

            DataContext = viewModel;
        }
    }
}
