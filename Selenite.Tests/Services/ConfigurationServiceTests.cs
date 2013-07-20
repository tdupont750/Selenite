using System;
using Selenite.Services.Implementation;
using Xunit;

namespace Selenite.Tests.Services
{
    public class ConfigurationServiceTests : ConfigurationService
    {
        protected override string GetAppSetting(string key)
        {
            return String.Empty;
        }

        [Fact]
        public void ChromeDriverPathTest()
        {
            var actual = ChromeDriverPath;
            Assert.Contains("ChromeDriver", actual);
        }

        [Fact]
        public void IeDriverPathTest()
        {
            var actual = IEDriverPath;
            Assert.Contains("IEDriver", actual);
        }

        [Fact]
        public void TestScriptsPathTest()
        {
            var actual = TestScriptsPath;
            Assert.Contains("TestScripts", actual);
        }

        [Fact]
        public void PhantomJsPathTest()
        {
            var actual = PhantomJsPath;
            Assert.Contains("PhantomJs", actual, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
