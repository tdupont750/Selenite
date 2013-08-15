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
        public void TestScriptsPathTest()
        {
            var actual = TestScriptsPath;
            Assert.Contains("TestScripts", actual);
        }
    }
}
