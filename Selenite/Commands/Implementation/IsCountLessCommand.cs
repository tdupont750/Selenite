using System.Collections.Generic;
using OpenQA.Selenium;
using Selenite.Commands.Base;
using Xunit;

namespace Selenite.Commands.Implementation
{
    public class IsCountLessCommand : MultipleSelectorCommandBase
    {
        public int Count { get; set; }

        public bool OrEqualTo { get; set; }

        protected override void Execute(IWebDriver driver, dynamic context, IList<IWebElement> elements)
        {
            if (OrEqualTo)
                Assert.True(elements.Count <= Count);
            else
                Assert.True(elements.Count < Count);
        }
    }
}