using Selenite.Enums;
using Selenite.Models;
using Xunit;
using Xunit.Extensions;

namespace Selenite.Tests.Browsers
{
    [SeleniteDriver(DriverType.Firefox)]
    public class Firefox : IUseFixture<SeleniteFixture>
    {
        public SeleniteFixture SeleniteFixture { get; private set; }

        public void SetFixture(SeleniteFixture data)
        {
            SeleniteFixture = data;
        }

#if FIREFOX
        [Theory, SeleniteData, CurrentDirectoryDomainOverride]
#else
        [Theory(Skip = "Not built for FIREFOX")]
#endif
        public void ExecuteTests(DriverType driverType, SeleniteTest test)
        {
            SeleniteFixture.ExecuteTest(driverType, test);
        }
    }
}