using OpenQA.Selenium;
using Selenite.Commands.Base;
using Xunit;

namespace Selenite.Commands.Implementation
{
    /// <summary>
    /// Checks if the selected element is selected (for example, a checkbox or radio button).
    /// </summary>
    public class IsSelectedCommand : SingleSelectorCommandBase
    {
        /// <summary>
        /// Boolean property (true/false) that reverses the way the command behaves.  Setting to true will return success if the element is NOT selected.
        /// Will default to false if not set.
        /// </summary>
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