using Selenite.Browsers;
using Selenite.Enums;
using Selenite.Models;
using Xunit.Extensions;

namespace Selenite.Tests.Browsers
{
    public class Chrome : BrowserBase
    {
        public override DriverType DriverType
        {
            get { return DriverType.Chrome; }
        }

#if CHROME
        [Theory]
#else
        [Theory(Skip = "Not built for Chrome")]
#endif
        [PropertyData(TestDataMember)]
        public void ExecuteTests(Test test)
        {
            ExecuteTest(test);
        }
    }
}