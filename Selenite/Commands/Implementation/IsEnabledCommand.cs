using System.ComponentModel;
using OpenQA.Selenium;
using Selenite.Commands.Base;
using Xunit;

namespace Selenite.Commands.Implementation
{
    [Description("Checks if the selected element is enabled.")]
    public class IsEnabledCommand : SingleSelectorCommandBase
    {
        [Description(@"Boolean property (true/false) that reverses the way the command behaves.  Setting to true will return success if the element is NOT enabled.
Will default to false if not set.")]
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