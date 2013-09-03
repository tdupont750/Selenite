using System;
using System.ComponentModel;
using OpenQA.Selenium;
using Selenite.Commands.Base;
using Xunit;

namespace Selenite.Commands.Implementation
{
    [Description(@"Compares the current selected element's text to the Text property.  Requires an element to be selected.
Checks for the Text property to be contained within the selected element's text, after trimming white-space.")]
    public class IsTextContainedCommand : SingleSelectorCommandBase
    {
        [Description(@"The string to be compared against the selected element's text.
This parameter is required.")]
        public string Text { get; set; }

        [Description(@"Boolean property (true/false) that can be used to make the text comparison case sensitive.
Will default to false if not set (ignoring case).")]
        public bool IsCaseSensitive { get; set; }

        [Description(@"Boolean property (true/false) that reverses the way the command behaves.  Setting to true will return success if Text is NOT contained in the selected element's text.
Will default to false if not set.")]
        public bool IsFalseExpected { get; set; }

        protected override void Execute(IWebDriver driver, dynamic context, IWebElement element)
        {
            var resolvedText = Test.ResolveMacros(Text);

            var stringComparison = IsCaseSensitive
                ? StringComparison.InvariantCulture
                : StringComparison.InvariantCultureIgnoreCase;

            if (IsFalseExpected)
                Assert.DoesNotContain(resolvedText.Trim(), element.Text.Trim(), stringComparison);
            else
                Assert.Contains(resolvedText.Trim(), element.Text.Trim(), stringComparison);
        }
    }
}