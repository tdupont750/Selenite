using System;
using System.ComponentModel;
using OpenQA.Selenium;
using Selenite.Commands.Base;
using Xunit;

namespace Selenite.Commands.Implementation
{
    [Description(@"Compares the current page title to the Title property.  Does not require an element to be selected.
Checks for an exact match, after trimming white-space.")]
    public class IsTitleEqualCommand : CommandBase
    {
        [Description(@"The string to be compared against the page title.
This parameter is required.")]
        public string Title { get; set; }

        [Description(@"Boolean property (true/false) that can be used to make the title comparison case sensitive.
Will default to false if not set (ignoring case).")]
        public bool IsCaseSensitive { get; set; }

        [Description(@"Boolean property (true/false) that reverses the way the command behaves.  Setting to true will return success if the title does NOT match.
Will default to false if not set.")]
        public bool IsFalseExpected { get; set; }

        public override void Execute(IWebDriver driver, dynamic context)
        {
            var resolvedTitle = Test.ResolveMacros(Title);

            var stringComparer = IsCaseSensitive
                ? StringComparer.InvariantCulture
                : StringComparer.InvariantCultureIgnoreCase;

            if (IsFalseExpected)
                Assert.NotEqual(resolvedTitle.Trim(), driver.Title.Trim(), stringComparer);
            else
                Assert.Equal(resolvedTitle.Trim(), driver.Title.Trim(), stringComparer);
        }
    }
}