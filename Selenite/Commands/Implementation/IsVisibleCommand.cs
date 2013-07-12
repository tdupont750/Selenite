using OpenQA.Selenium;
using Selenite.Commands.Base;
using Xunit;

namespace Selenite.Commands.Implementation
{
    /// <summary>
    /// Checks if the selected element is visible.
    /// </summary>
    public class IsVisibleCommand : SingleSelectorCommandBase
    {
        /// <summary>
        /// Boolean property (true/false) that reverses the way the command behaves.  Setting to true will cause a hidden element to return success, and vice versa.
        /// Will default to false if not set.
        /// </summary>
        public bool IsFalseExpected { get; set; }

        protected override void Execute(IWebDriver driver, dynamic contetx, IWebElement element)
        {
            if (IsFalseExpected)
                Assert.False(element.Displayed);
            else
                Assert.True(element.Displayed);
        }
    }
}