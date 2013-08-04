using Selenite.Browsers;
using Selenite.Enums;
using Selenite.Models;
using Xunit.Extensions;

namespace Selenite.Tests.Browsers
{
    public class Firefox : BrowserBase
    {
        public override DriverType DriverType
        {
            get { return DriverType.Firefox; }
        }

#if FIREFOX
        [Theory, BrowserData]
#else
        [Theory(Skip = "Not built for FireFox")]
#endif
        public void ExecuteTests(Test test)
        {
            ExecuteTest(test);
        }
    }
}