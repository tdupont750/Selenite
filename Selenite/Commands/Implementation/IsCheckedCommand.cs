using OpenQA.Selenium;
using Selenite.Commands.Base;
using Xunit;

namespace Selenite.Commands.Implementation
{
    /// <summary>
    /// Checks if the selected element is checked (for example, a checkbox).  Requires an element to be selected.
    /// </summary>
    public class IsCheckedCommand : SingleSelectorCommandBase
    {
        /// <summary>
        /// Boolean property (true/false) that reverses the way the command behaves.  Setting to true will return success if the element is NOT checked.
        /// Will default to false if not set.
        /// </summary>
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