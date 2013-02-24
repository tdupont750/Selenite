using System;
using OpenQA.Selenium;
using Selenite.Commands.Base;
using Xunit;

namespace Selenite.Commands.Implementation
{
    public class IsAttributeContainedCommand : SingleSelectorCommandBase
    {
        public string Attribute { get; set; }

        public string Value { get; set; }

        public bool IsCaseSensitive { get; set; }

        public bool IsFalseExpected { get; set; }

        protected override void Execute(IWebDriver driver, dynamic context, IWebElement element)
        {
            var stringComparison = IsCaseSensitive
                ? StringComparison.InvariantCulture
                : StringComparison.InvariantCultureIgnoreCase;

            var attribute = element
                .GetAttribute(Attribute)
                .Trim();

            if (IsFalseExpected)
                Assert.DoesNotContain(Value.Trim(), attribute, stringComparison);
            else
                Assert.Contains(Value.Trim(), attribute, stringComparison);
        }
    }
}