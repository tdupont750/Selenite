using System;
using Selenite.Commands.Implementation;
using Xunit;
using Xunit.Extensions;

namespace Selenite.Tests.Commands
{
    public class SendKeyCommandTests
    {
        [Theory]
        [InlineData("Enter")]
        [InlineData("bACKSPACE")]
        [InlineData("tab")]
        [InlineData("SUBTRACT")]
        public void Valid(string key)
        {
            var command = new DoSendKeyCommand {Key = key};
            command.Validate();
        }

        [Theory]
        [InlineData("")]
        [InlineData("Invalid")]
        public void Invalid(string key)
        {
            var command = new DoSendKeyCommand { Key = key };
            Assert.Throws<ArgumentException>(() => command.Validate());
        }
    }
}
