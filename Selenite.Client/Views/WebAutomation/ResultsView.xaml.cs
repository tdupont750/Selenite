using Selenite.Client.ViewModels.WebAutomation;

namespace Selenite.Client.Views.WebAutomation
{
    /// <summary>
    /// Interaction logic for ResultsView.xaml
    /// </summary>
    public partial class ResultsView
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResultsView"/> class.
        /// </summary>
        public ResultsView()
        {
            InitializeComponent();
        }

        // Can't bind to the treeviews selected item property so this is a hacky workaround.
        private void TreeView_SelectedItemChanged(object sender, System.Windows.RoutedPropertyChangedEventArgs<object> e)
        {
            var viewModel = (ResultsViewModel) DataContext;

            var testResult = e.NewValue as TestResultViewModel;

            if (testResult != null)
            {
                viewModel.SelectedTestResult = testResult;
            }
        }
    }
}