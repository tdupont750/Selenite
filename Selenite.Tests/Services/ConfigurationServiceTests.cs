using Selenite.Services.Implementation;
using Xunit;
using Xunit.Extensions;

namespace Selenite.Tests.Services
{
    public class ConfigurationServiceTests
    {
        [Theory]
        [InlineData("ChromeDriver")]
        [InlineData("IEDriver")]
        public void FindDriverPath(string driverName)
        {
            var configurationService = new ConfigurationService();

            string driverPath;
            var result = configurationService.FindDriverPath(driverName, out driverPath);

            Assert.True(result);
            Assert.Contains(driverName, driverPath);
        }
    }
}
