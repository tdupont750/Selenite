using Selenite.Enums;
using Selenite.Models;
using Xunit;
using Xunit.Extensions;

namespace Selenite.Client.Browsers
{
    [SeleniteDriver(DriverType.PhantomJs)]
    public class PhantomJs : IUseFixture<SeleniteFixture>
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