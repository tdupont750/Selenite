using System;
using System.ComponentModel;
using OpenQA.Selenium;
using Selenite.Commands.Base;
using Xunit;

namespace Selenite.Commands.Implementation
{
    [Description(@"Checks if the selected dropdown is set to a particular option.")]
    public class IsDropdownSelectedCommand : SingleSelectorCommandBase
    {
        [Description(@"The text of the intended selected option.
Either Text or Value must be set.")]
        public string Text { get; set; }

        [Description(@"The value of the intended selected option.
Either Text or Value must be set.
Will be ignored if Text is used.")]
        public string Value { get; set; }

        [Description(@"Boolean property (true/false) that can be used to make the text/value comparison case sensitive.
Will default to false if not set (ignoring case).")]
        public bool IsCaseSensitive { get; set; }

        [Description(@"Boolean property (true/false) that reverses the way the command behaves.  Setting to true will return success if the selected option does NOT match.
Will default to false if not set.")]
        public bool IsFalseExpected { get; set; }

        public override void Validate()
        {
            if (String.IsNullOrWhiteSpace(Text) ^ String.IsNullOrWhiteSpace(Value))
                return;

            throw new ArgumentException("Must have Text or Value");
        }

        protected override void Execute(IWebDriver driver, dynamic context, IWebElement element)
        {
            var resolvedText = Test.ResolveMacros(Text);
            var resolvedValue = Test.ResolveMacros(Value);

            var isValue = String.IsNullOrWhiteSpace(resolvedText);

            var stringComparison = IsCaseSensitive
                ? StringComparison.InvariantCulture
                : StringComparison.InvariantCultureIgnoreCase;

            var option = isValue
                ? element.GetOptionByValue(resolvedValue, stringComparison)
                : element.GetOptionByText(resolvedText, stringComparison);

            if (option == null)
            {
                var key = isValue ? "Value" : "Text";
                var value = isValue ? resolvedValue : resolvedText;
                var message = String.Format("Unable to locate option by {0}: {1}", key, value);
                throw new Exception(message);
            }

            var isSelected = option.IsSelected();

            if (IsFalseExpected)
                Assert.False(isSelected);
            else
                Assert.True(isSelected);
        }
    }
}