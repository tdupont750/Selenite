using System;
using OpenQA.Selenium;
using Selenite.Commands.Base;
using Xunit;

namespace Selenite.Commands.Implementation
{
    public class IsTitleContainedCommand : CommandBase
    {
        public string Title { get; set; }

        public bool IsCaseSensitive { get; set; }

        public bool IsFalseExpected { get; set; }

        public override void Execute(IWebDriver driver, dynamic context)
        {
            var stringComparison = IsCaseSensitive
                ? StringComparison.InvariantCulture
                : StringComparison.InvariantCultureIgnoreCase;

            if (IsFalseExpected)
                Assert.DoesNotContain(Title.Trim(), driver.Title.Trim(), stringComparison);
            else
                Assert.Contains(Title.Trim(), driver.Title.Trim(), stringComparison);
        }
    }
}