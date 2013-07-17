using Selenite.Models;

namespace Selenite.Client.ViewModels.WebAutomation
{
    public class TestResultViewModel : ViewModelBase
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public ResultStatus Status { get; set; }
        public string ResultOutput { get; set; }
        public string StackTrace { get; set; }
    }
}
