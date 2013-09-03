using System.Collections.Generic;
using System.ComponentModel;
using OpenQA.Selenium;
using Selenite.Commands.Base;
using Xunit;

namespace Selenite.Commands.Implementation
{
    [Description(@"Checks if the number of selected elements is GREATER than the specified count.")]
    public class IsCountGreaterCommand : MultipleSelectorCommandBase
    {
        [Description(@"The integer value to compare to the number of selected elements.
This parameter is required.")]
        public int Count { get; set; }

        [Description(@"Alters the command to return true if Count is greater than or equal to the selected elements.")]
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