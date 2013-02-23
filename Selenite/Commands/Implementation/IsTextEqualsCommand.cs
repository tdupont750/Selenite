using System;
using OpenQA.Selenium;
using Selenite.Commands.Base;
using Xunit;

namespace Selenite.Commands.Implementation
{
    public class IsTextEqualsCommand : SingleSelectorCommandBase
    {
        public string Text { get; set; }

        public bool IsCaseSensitive { get; set; }

        public bool IsFalseExpected { get; set; }

        protected override void Execute(IWebDriver driver, dynamic context, IWebElement element)
        {
            var stringComparer = IsCaseSensitive
                ? StringComparer.InvariantCulture
                : StringComparer.InvariantCultureIgnoreCase;

            if (IsFalseExpected)
                Assert.NotEqual(Text.Trim(), element.Text.Trim(), stringComparer);
            else
                Assert.Equal(Text.Trim(), element.Text.Trim(), stringComparer);
        }
    }
}