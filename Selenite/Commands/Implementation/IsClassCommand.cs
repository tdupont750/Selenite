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

            var @class = element.GetAttribute("class");

            if (IsFalseExpected)
                Assert.NotEqual(@class, Class, stringComparer);
            else
                Assert.Equal(@class, Class, stringComparer);
        }
    }
}