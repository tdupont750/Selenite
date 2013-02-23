using System.Collections.Generic;
using OpenQA.Selenium;
using Selenite.Commands.Base;
using Xunit;

namespace Selenite.Commands.Implementation
{
    public class IsCountGreaterCommand : MultipleSelectorCommandBase
    {
        public int Count { get; set; }

        protected override void Execute(IWebDriver driver, dynamic context, IList<IWebElement> elements)
        {
            Assert.True(elements.Count > Count);
        }
    }
}