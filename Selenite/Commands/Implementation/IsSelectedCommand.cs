using OpenQA.Selenium;
using Selenite.Commands.Base;
using Selenite.Extensions;
using Xunit;

namespace Selenite.Commands.Implementation
{
    public class IsSelectedCommand : SingleSelectorCommandBase
    {
        public bool IsFalseExpected { get; set; }

        protected override void Execute(IWebDriver driver, dynamic contetx, IWebElement element)
        {
            var isSelected = element.IsSelected();

            if (IsFalseExpected)
                Assert.False(isSelected);
            else
                Assert.True(isSelected);
        }
    }
}