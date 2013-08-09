using System.Windows.Input;
using Selenite.Models;

namespace Selenite.Client.ViewModels.WebAutomation
{
    public class TestResultViewModel : ViewModelBase
    {
        public TestResultViewModel()
        {
            OpenProcessCommand = new RelayCommand(parameter => System.Diagnostics.Process.Start(parameter.ToString()));
        }

        public string Name { get; set; }
        public string Url { get; set; }
        public ResultStatus Status { get; set; }
        public string ResultOutput { get; set; }
        public string StackTrace { get; set; }
        public string Browser { get; set; }
        public string ScreenshotPath { get; set; }

        public ICommand OpenProcessCommand { get; set; }
    }
}
