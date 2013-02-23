using OpenQA.Selenium;
using Selenite.Commands.Base;
using Xunit;

namespace Selenite.Commands.Implementation
{
    public class IsVisibleCommand : SingleSelectorCommandBase
    {
        public bool IsNot { get; set; }

        protected override void Execute(IWebDriver driver, dynamic contetx, IWebElement element)
        {
            if (IsNot)
                Assert.False(element.Displayed);
            else
                Assert.True(element.Displayed);
        }
    }
}