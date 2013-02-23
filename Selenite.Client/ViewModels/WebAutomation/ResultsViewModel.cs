using System.Windows.Input;

namespace Selenite.Client.ViewModels.WebAutomation
{
    public class ResultsViewModel : ViewModelBase
    {
        public ResultsViewModel()
        {
            UseFirefox = true;

            RunTestsCommand = new RelayCommand(RunTests,
                                               t =>
                                               UseFirefox || UseChrome || UseInternetExplorer);
        }

        public bool UseFirefox
        {
            get { return Get(() => UseFirefox); }
            set { Set(value, () => UseFirefox); }
        }

        public bool UseChrome
        {
            get { return Get(() => UseChrome); }
            set { Set(value, () => UseChrome); }
        }

        public bool UseInternetExplorer
        {
            get { return Get(() => UseInternetExplorer); }
            set { Set(value, () => UseInternetExplorer); }
        }

        public string ResultText
        {
            get { return Get(() => ResultText); }
            set { Set(value, () => ResultText); }
        }

        public ICommand RunTestsCommand { get; set; }

        private void RunTests(object parameter)
        {
            ResultText = string.Empty;

            // TODO: Run tests here
        }
    }
}