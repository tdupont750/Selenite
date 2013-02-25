using System;
using OpenQA.Selenium;
using Selenite.Commands.Base;
using Xunit;

namespace Selenite.Commands.Implementation
{
    public class IsAttributeEqualCommand : SingleSelectorCommandBase
    {
        public string Attribute { get; set; }

        public string Value { get; set; }

        public bool IsCaseSensitive { get; set; }

        public bool IsFalseExpected { get; set; }

        protected override void Execute(IWebDriver driver, dynamic context, IWebElement element)
        {
            var stringComparer = IsCaseSensitive
                ? StringComparer.InvariantCulture
                : StringComparer.InvariantCultureIgnoreCase;

            var attribute = element
                .GetAttribute(Attribute)
                .Trim();

            if (IsFalseExpected)
                Assert.NotEqual(Value.Trim(), attribute, stringComparer);
            else
                Assert.Equal(Value.Trim(), attribute, stringComparer);
        }
    }
}