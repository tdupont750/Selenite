using System.Collections.Generic;
using OpenQA.Selenium;
using Selenite.Commands.Base;
using Xunit;

namespace Selenite.Commands.Implementation
{
    /// <summary>
    /// Checks if the number of selected elements is GREATER than the specified count.
    /// </summary>
    public class IsCountGreaterCommand : MultipleSelectorCommandBase
    {
        /// <summary>
        /// The integer value to compare to the number of selected elements.
        /// This parameter is required.
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// Alters the command to return true if Count is greater than or equal to the selected elements.
        /// </summary>
        public bool OrEqualTo { get; set; }

        protected override void Execute(IWebDriver driver, dynamic context, IList<IWebElement> elements)
        {
            if (OrEqualTo)
                Assert.True(elements.Count >= Count);
            else
                Assert.True(elements.Count > Count);
        }
    }
}