using Selenite.Enums;
using Selenite.Models;
using Xunit;
using Xunit.Extensions;

namespace Selenite.Tests.Browsers
{
    [SeleniteDriver(DriverType.InternetExplorer)]
    public class InternetExplorer : IUseFixture<SeleniteFixture>
    {
        public SeleniteFixture SeleniteFixture { get; private set; }

        public void SetFixture(SeleniteFixture data)
        {
            SeleniteFixture = data;
        }

#if IE
        [Theory, SeleniteData, CurrentDirectoryDomainOverride]
#else
        [Theory(Skip = "Not built for IE")]
#endif
        public void ExecuteTests(DriverType driverType, SeleniteTest test)
        {
            SeleniteFixture.ExecuteTest(driverType, test);
        }
    }
}