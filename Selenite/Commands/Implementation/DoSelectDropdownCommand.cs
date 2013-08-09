using System;
using OpenQA.Selenium;
using Selenite.Commands.Base;
using Selenite.Extensions;

namespace Selenite.Commands.Implementation
{
    /// <summary>
    /// Selects an option with the selected dropdown. Requires an element to be selected.
    /// </summary>
    public class DoSelectDropdownCommand : SingleSelectorCommandBase
    {
        /// <summary>
        /// The text of the intended selected option.
        /// Either Text or Value must be set.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// The value of the intended selected option.
        /// Either Text or Value must be set.
        /// Will be ignored if Text is used.
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Boolean property (true/false) that can be used to make the text/value comparison case sensitive.
        /// Will default to false if not set (ignoring case).
        /// </summary>
        public bool IsCaseSensitive { get; set; }

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

            option.Click();
        }
    }
}