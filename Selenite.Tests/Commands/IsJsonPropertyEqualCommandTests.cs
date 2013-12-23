using Selenite.Commands.Implementation;
using Selenite.Models;
using Selenite.Services.Implementation;
using Xunit.Extensions;

namespace Selenite.Tests.Commands
{
    public class IsJsonPropertyEqualCommandTests
    {
        [Theory]
        [InlineData("{ \"test\": true }", "test", "true")]
        [InlineData("{ \"test\": true }", "test", "True")]
        [InlineData("{ \"test\": \"batman!\" }", "test", "batman!")]
        public void Valid(string pageSource, string propertyName, string propertyValue)
        {
            var command = new IsJsonPropertyEqualCommand
            {
                Test = new SeleniteTest(),
                PropertyName = propertyName,
                PropertyValue = propertyValue,
            };

            var driver = new WebClientDriver
            {
                PageSource = pageSource,
            };

            command.Execute(driver, null);
        }

        [Theory]
        [InlineData("", "test", "true")]
        [InlineData("{ \"test\": true }", "test", "false")]
        [InlineData("{ \"test\": \"batman!\" }", "test", "riddler?")]
        public void InValid(string pageSource, string propertyName, string propertyValue)
        {
            var command = new IsJsonPropertyEqualCommand
            {
                Test = new SeleniteTest(),
                PropertyName = propertyName,
                PropertyValue = propertyValue,
            };

            var driver = new WebClientDriver
            {
                PageSource = pageSource,
            };

            command.Execute(driver, null);
        }
    }
}
