using Selenite.Browsers.Base;
using Selenite.Enums;
using Selenite.Models;
using Xunit.Extensions;

namespace Selenite.Browsers
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