using System;
using OpenQA.Selenium;
using Selenite.Commands.Base;
using Selenite.Extensions;

namespace Selenite.Commands.Implementation
{
    public class DoSelectDropdownCommand : SingleSelectorCommandBase
    {
        public string Text { get; set; }

        public string Value { get; set; }

        public bool IsCaseSensitive { get; set; }

        public override void Validate()
        {
            if (String.IsNullOrWhiteSpace(Text) ^ String.IsNullOrWhiteSpace(Value))
                return;

            throw new ArgumentException("Must have Text or Value");
        }

        protected override void Execute(IWebDriver driver, dynamic context, IWebElement element)
        {
            var isValue = String.IsNullOrWhiteSpace(Text);

            var stringComparison = IsCaseSensitive
                ? StringComparison.InvariantCulture
                : StringComparison.InvariantCultureIgnoreCase;

            var option = isValue
                ? element.GetOptionByValue(Value, stringComparison)
                : element.GetOptionByText(Text, stringComparison);

            if (option == null)
            {
                var key = isValue ? "Value" : "Text";
                var value = isValue ? Value : Text;
                var message = String.Format("Unable to locate option by {0}: {1}", key, value);
                throw new Exception(message);
            }

            option.Click();
        }
    }
}