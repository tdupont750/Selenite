using Selenite.Browsers;
using Selenite.Enums;
using Selenite.Models;
using Xunit.Extensions;

namespace Selenite.Client.Browsers
{
    public class PhantomJs : BrowserBase
    {
        public override DriverType DriverType
        {
            get { return DriverType.PhantomJs; }
        }

        [Theory]
        [PropertyData(TestDataMember)]
        public void ExecuteTests(Test test)
        {
            ExecuteTest(test);
        }
    }
}