using System;
using System.ComponentModel;
using OpenQA.Selenium;
using Selenite.Commands.Base;
using Xunit;

namespace Selenite.Commands.Implementation
{
    [Description(@"Compares the current selected element's text to the Text property.  Requires an element to be selected.
Checks for an exact match, after trimming white-space.")]
    public class IsTextEqualCommand : SingleSelectorCommandBase
    {
        [Description(@"The string to be compared against the selected element's text.
This parameter is required.")]
        public string Text { get; set; }

        [Description(@"Boolean property (true/false) that can be used to make the text comparison case sensitive.
Will default to false if not set (ignoring case).")]
        public bool IsCaseSensitive { get; set; }

        [Description(@"Boolean property (true/false) that reverses the way the command behaves.  Setting to true will return success if the text does NOT match.
Will default to false if not set.")]
        public bool IsFalseExpected { get; set; }

        protected override void Execute(IWebDriver driver, dynamic context, IWebElement element)
        {
            var resolvedText = Test.ResolveMacros(Text);

            var stringComparer = IsCaseSensitive
                ? StringComparer.InvariantCulture
                : StringComparer.InvariantCultureIgnoreCase;

            if (IsFalseExpected)
                Assert.NotEqual(resolvedText.Trim(), element.Text.Trim(), stringComparer);
            else
                Assert.Equal(resolvedText.Trim(), element.Text.Trim(), stringComparer);
        }
    }
}