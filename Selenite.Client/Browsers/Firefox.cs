using Selenite.Browsers;
using Selenite.Enums;
using Selenite.Models;
using Xunit.Extensions;

namespace Selenite.Client.Browsers
{
    public class Firefox : BrowserBase
    {
        public override DriverType DriverType
        {
            get { return DriverType.Firefox; }
        }

        [Theory, BrowserData]
        public void ExecuteTests(Test test)
        {
            ExecuteTest(test);
        }
    }
}