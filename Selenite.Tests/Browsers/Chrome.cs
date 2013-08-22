using Selenite.Enums;
using Selenite.Models;
using Xunit;
using Xunit.Extensions;

namespace Selenite.Tests.Browsers
{
    /*public class Chrome : BrowserBase
    {
        public override DriverType DriverType
        {
            get { return DriverType.Chrome; }
        }

#if CHROME
        [Theory, BrowserData]
#else
        [Theory(Skip = "Not built for Chrome")]
#endif
        public void ExecuteTests(SeleniteTest test)
        {
            ExecuteTest(test);
        }
    }*/

    [SeleniteDriver(DriverType.Chrome)]
    public class BrowserTests : IUseFixture<SeleniteFixture>
    {
        public SeleniteFixture SeleniteFixture { get; private set; }

        public void SetFixture(SeleniteFixture data)
        {
            SeleniteFixture = data;
        }

        [Theory, SeleniteData]
        public void ExecuteTests(DriverType driverType, SeleniteTest test)
        {
            SeleniteFixture.ExecuteTest(driverType, test);
        }
    }
}