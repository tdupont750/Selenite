using Selenite.Browsers.Base;
using Selenite.Enums;
using Selenite.Models;
using Xunit.Extensions;

namespace Selenite.Browsers
{
    public class Firefox : BrowserBase
    {
        public override DriverType DriverType
        {
            get { return DriverType.Firefox; }
        }

#if FIREFOX
        [Theory]
#else
        [Theory(Skip = "Not built for FireFox")]
#endif
        [PropertyData(TestDataMember)]
        public void ExecuteTests(Test test)
        {
            ExecuteTest(test);
        }
    }
}