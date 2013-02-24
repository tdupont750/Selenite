using System;
using OpenQA.Selenium;
using Selenite.Commands.Base;
using Xunit;

namespace Selenite.Commands.Implementation
{
    public class IsTextContainedCommand : SingleSelectorCommandBase
    {
        public string Text { get; set; }

        public bool IsCaseSensitive { get; set; }

        public bool IsFalseExpected { get; set; }

        protected override void Execute(IWebDriver driver, dynamic context, IWebElement element)
        {
            var stringComparison = IsCaseSensitive
                ? StringComparison.InvariantCulture
                : StringComparison.InvariantCultureIgnoreCase;

            if (IsFalseExpected)
                Assert.DoesNotContain(Text.Trim(), element.Text.Trim(), stringComparison);
            else
                Assert.Contains(Text.Trim(), element.Text.Trim(), stringComparison);
        }
    }
}