using System;
using System.ComponentModel;
using OpenQA.Selenium;
using Selenite.Commands.Base;
using Xunit;

namespace Selenite.Commands.Implementation
{
    [Description(@"Checks to see if the selected element's Attribute is equal to the specified Value.  Requires an element to be selected.")]
    public class IsAttributeEqualCommand : SingleSelectorCommandBase
    {
        [Description(@"The name of the Attribute in the selected element to be checked.
This parameter is required.")]
        public string Attribute { get; set; }

        [Description(@"The string to be compared against the selected element's attribute.
This parameter is required.")]
        public string Value { get; set; }

        [Description(@"Boolean property (true/false) that can be used to make the value comparison case sensitive.
Will default to false if not set (ignoring case).")]
        public bool IsCaseSensitive { get; set; }

        [Description(@"Boolean property (true/false) that reverses the way the command behaves.  Setting to true will return success if Value is NOT equal to the selected element's Attribute.
Will default to false if not set.")]
        public bool IsFalseExpected { get; set; }

        protected override void Execute(IWebDriver driver, dynamic context, IWebElement element)
        {
            var stringComparer = IsCaseSensitive
                ? StringComparer.InvariantCulture
                : StringComparer.InvariantCultureIgnoreCase;

            var resolvedAttribute = Test.ResolveMacros(Attribute);
            var resolvedValue = Test.ResolveMacros(Value);

            var attribute = (element.GetAttribute(resolvedAttribute) ?? string.Empty)
                .Trim();

            if (IsFalseExpected)
                Assert.NotEqual(resolvedValue.Trim(), attribute, stringComparer);
            else
                Assert.Equal(resolvedValue.Trim(), attribute, stringComparer);
        }
    }
}