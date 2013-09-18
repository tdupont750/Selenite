using Common.ViewModels;
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

        // Can't bind to the treeviews selected item property so this is a hacky workaround.
        private void TreeView_SelectedItemChanged(object sender, System.Windows.RoutedPropertyChangedEventArgs<object> e)
        {
            var viewModel = (TestResultsViewModel)DataContext;

            var testResult = e.NewValue as ViewModelBase;

            if (testResult != null)
            {
                viewModel.SelectedTestResult = testResult;
            }
        }
    }
}
