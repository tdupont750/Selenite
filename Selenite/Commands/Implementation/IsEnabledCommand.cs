using OpenQA.Selenium;
using Selenite.Commands.Base;
using Xunit;

namespace Selenite.Commands.Implementation
{
    public class IsEnabledCommand : SingleSelectorCommandBase
    {
        public bool IsFalseExpected { get; set; }

        protected override void Execute(IWebDriver driver, dynamic contetx, IWebElement element)
        {
            if (IsFalseExpected)
                Assert.False(element.Enabled);
            else
                Assert.True(element.Enabled);
        }
    }
}