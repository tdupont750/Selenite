using System;
using Newtonsoft.Json;
using Selenite.Commands.Implementation;
using Selenite.Models;
using Selenite.Services.Implementation;
using Xunit;
using Xunit.Extensions;

namespace Selenite.Tests.Commands
{
    public class IsJsonCommandTests
    {
        [Theory]
        [InlineData("{ \"test\": true }")]
        [InlineData("")]
        public void Valid(string pageSource)
        {
            var command = new IsJsonCommand {Test = new SeleniteTest()};
            var driver = new WebClientDriver
                {
                    PageSource = pageSource,
                };

            command.Execute(driver, null);
        }

        [Theory]
        [InlineData("Invalid")]
        public void Invalid(string pageSource)
        {
            var command = new IsJsonCommand { Test = new SeleniteTest() };
            var driver = new WebClientDriver
            {
                PageSource = pageSource,
            };

            Assert.Throws<JsonReaderException>(() => command.Execute(driver, null));
        }
    }
}
