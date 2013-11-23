using Selenite.Services.Implementation;
using Xunit;
using Xunit.Extensions;

namespace Selenite.Tests.Drivers
{
    public class WebClientDriverTests
    {
        [Theory]
        [InlineData("http://github.com", "<title>GitHub")]
        public void NavigateTo(string url, string expectedText)
        {
            var webClientDriver = new WebClientDriver();
            webClientDriver.Navigate().GoToUrl(url);

            Assert.Equal(url, webClientDriver.Url);
            Assert.Contains(expectedText, webClientDriver.PageSource);
        }
    }
}
