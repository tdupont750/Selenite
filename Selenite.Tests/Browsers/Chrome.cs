using Selenite.Enums;
using Selenite.Models;
using Xunit;
using Xunit.Extensions;

namespace Selenite.Tests.Browsers
{
    [SeleniteDriver(DriverType.Chrome)]
    public class Chrome : IUseFixture<SeleniteFixture>
    {
        public SeleniteFixture SeleniteFixture { get; private set; }

        public void SetFixture(SeleniteFixture data)
        {
            SeleniteFixture = data;
        }

#if CHROME
        [Theory, SeleniteData, CurrentDirectoryDomainOverride]
#else
        [Theory(Skip = "Not built for CHROME")]
#endif
        public void ExecuteTests(DriverType driverType, SeleniteTest test)
        {
            SeleniteFixture.ExecuteTest(driverType, test);
        }
    }
}