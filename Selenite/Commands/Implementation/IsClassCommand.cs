using System;
using OpenQA.Selenium;
using Selenite.Commands.Base;
using Xunit;

namespace Selenite.Commands.Implementation
{
    public class IsClassCommand : SingleSelectorCommandBase
    {
        public string Class { get; set; }

        public bool IsCaseSensitive { get; set; }

        public bool IsFalseExpected { get; set; }

        protected override void Execute(IWebDriver driver, dynamic contetx, IWebElement element)
        {
            var stringComparer = IsCaseSensitive
                ? StringComparer.InvariantCulture
                : StringComparer.InvariantCultureIgnoreCase;

            var classes = element.GetAttribute("class").Split(' ');

            if (IsFalseExpected)
                Assert.DoesNotContain(Class, classes, stringComparer);
            else
                Assert.Contains(Class, classes, stringComparer);
        }
    }
}