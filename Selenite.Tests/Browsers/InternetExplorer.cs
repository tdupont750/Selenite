using Selenite.Browsers;
using Selenite.Enums;
using Selenite.Models;
using Xunit.Extensions;

namespace Selenite.Tests.Browsers
{
    public class InternetExplorer : BrowserBase
    {
        public override DriverType DriverType
        {
            get { return DriverType.InternetExplorer; }
        }

#if IE
        [Theory, BrowserData]
#else
        [Theory(Skip = "Not built for IE")]
#endif
        public void ExecuteTests(Test test)
        {
            ExecuteTest(test);
        }
    }
}