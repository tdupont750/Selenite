using System;
using OpenQA.Selenium;
using Selenite.Commands.Base;
using Xunit;

namespace Selenite.Commands.Implementation
{
    /// <summary>
    /// Compares the current page title to the Title property.  Does not require an element to be selected.
    /// Checks for the Title property to be contained within the page title, after trimming white-space.
    /// </summary>
    public class IsTitleContainedCommand : CommandBase
    {
        /// <summary>
        /// The string to be compared against the page title.
        /// This parameter is required.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Boolean property (true/false) that can be used to make the title comparison case sensitive.
        /// Will default to false if not set (ignoring case).
        /// </summary>
        public bool IsCaseSensitive { get; set; }

        /// <summary>
        /// Boolean property (true/false) that reverses the way the command behaves.  Setting to true will return success if Title is NOT contained in the page title.
        /// Will default to false if not set.
        /// </summary>
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