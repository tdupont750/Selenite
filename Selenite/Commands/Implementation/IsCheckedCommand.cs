using OpenQA.Selenium;
using Selenite.Commands.Base;
using Selenite.Extensions;
using Xunit;

namespace Selenite.Commands.Implementation
{
    public class IsCheckedCommand : SingleSelectorCommandBase
    {
        public bool IsFalseExpected { get; set; }

        protected override void Execute(IWebDriver driver, dynamic contetx, IWebElement element)
        {
            var isChecked = element.IsChecked();

            if (IsFalseExpected)
                Assert.False(isChecked);
            else
                Assert.True(isChecked);
        }
    }
}