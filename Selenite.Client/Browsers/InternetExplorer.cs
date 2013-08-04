using Selenite.Browsers;
using Selenite.Enums;
using Selenite.Models;
using Xunit.Extensions;

namespace Selenite.Client.Browsers
{
    public class InternetExplorer : BrowserBase
    {
        public override DriverType DriverType
        {
            get { return DriverType.InternetExplorer; }
        }

        [Theory, BrowserData]
        public void ExecuteTests(Test test)
        {
            ExecuteTest(test);
        }
    }
}