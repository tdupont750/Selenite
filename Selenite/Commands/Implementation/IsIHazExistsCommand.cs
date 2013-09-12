using System.ComponentModel;
using OpenQA.Selenium;
using Selenite.Commands.Base;
using Xunit;

namespace Selenite.Commands.Implementation
{
    [Description("Checks if the selected element exists on the page.")]
    public class IsExistsCommand : SingleSelectorCommandBase
    {
        public IsExistsCommand()
        {
            AllowNullElement = true;
        }

        [Description(@"Boolean property (true/false) that reverses the way the command behaves.  Setting to true will cause a non-existing element to return success, and vice versa.
Will default to false if not set.")]
        public bool IsFalseExpected { get; set; }

        protected override void Execute(IWebDriver driver, dynamic context, IWebElement element)
        {
            var elementExists = element != null;
            if (IsFalseExpected)
                Assert.False(elementExists);
            else
                Assert.True(elementExists);
        }
    }
}