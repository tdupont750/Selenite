using OpenQA.Selenium;
using Selenite.Commands.Base;
using Xunit;

namespace Selenite.Commands.Implementation
{
    /// <summary>
    /// Checks if the selected element is enabled.
    /// </summary>
    public class IsEnabledCommand : SingleSelectorCommandBase
    {
        /// <summary>
        /// Boolean property (true/false) that reverses the way the command behaves.  Setting to true will return success if the element is NOT enabled.
        /// Will default to false if not set.
        /// </summary>
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