using Selenite.Client.TestResults.ViewModels;

namespace Selenite.Client.TestResults.Views
{
    /// <summary>
    /// Interaction logic for TestResultsView.xaml
    /// </summary>
    public partial class TestResultsView
    {
        public TestResultsView(TestResultsViewModel viewModel)
        {
            InitializeComponent();

            DataContext = viewModel;
        }
    }
}
