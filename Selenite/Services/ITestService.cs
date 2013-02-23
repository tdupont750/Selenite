using Selenite.Browsers.Base;
using Selenite.Models;

namespace Selenite.Services
{
    public interface ITestService
    {
        void ExecuteTest(BrowserBase browser, Test test);
    }
}