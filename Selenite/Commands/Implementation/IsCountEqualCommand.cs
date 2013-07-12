using System.Collections.Generic;
using OpenQA.Selenium;
using Selenite.Commands.Base;
using Xunit;

namespace Selenite.Commands.Implementation
{
    /// <summary>
    /// Checks if the number of selected elements is EQUAL TO the specified count.
    /// </summary>
    public class IsCountEqualCommand : MultipleSelectorCommandBase
    {
        /// <summary>
        /// The integer value to compare to the number of selected elements.
        /// This parameter is required.
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// Boolean property (true/false) that reverses the way the command behaves.  Setting to true will return success if the number of selected elements is NOT equal to Count.
        /// Will default to false if not set.
        /// </summary>
        public bool IsFalseExpected { get; set; }

        protected override void Execute(IWebDriver driver, dynamic context, IList<IWebElement> elements)
        {
            if (IsFalseExpected)
                Assert.NotEqual(Count, elements.Count);
            else
                Assert.Equal(Count, elements.Count);
        }
    }
}