using Selenite.Browsers;
using Selenite.Enums;
using Selenite.Models;
using Xunit.Extensions;

namespace Selenite.Client.Browsers
{
    public class Chrome : BrowserBase
    {
        public override DriverType DriverType
        {
            get { return DriverType.Chrome; }
        }

        [Theory]
        [PropertyData(TestDataMember)]
        public void ExecuteTests(Test test)
        {
            ExecuteTest(test);
        }
    }
}