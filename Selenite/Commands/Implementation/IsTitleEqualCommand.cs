using System;
using OpenQA.Selenium;
using Selenite.Commands.Base;
using Xunit;

namespace Selenite.Commands.Implementation
{
    public class IsTitleEqualCommand : CommandBase
    {
        public string Title { get; set; }

        public bool IsCaseSensitive { get; set; }

        public bool IsFalseExpected { get; set; }

        public override void Execute(IWebDriver driver, dynamic context)
        {
            var stringComparer = IsCaseSensitive
                ? StringComparer.InvariantCulture
                : StringComparer.InvariantCultureIgnoreCase;

            if (IsFalseExpected)
                Assert.NotEqual(Title.Trim(), driver.Title.Trim(), stringComparer);
            else
                Assert.Equal(Title.Trim(), driver.Title.Trim(), stringComparer);
        }
    }
}