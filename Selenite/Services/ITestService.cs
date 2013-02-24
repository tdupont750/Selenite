using Selenite.Browsers;
using Selenite.Models;

namespace Selenite.Services
{
    public interface ITestService
    {
        void ExecuteTest(BrowserBase browser, Test test);
    }
}