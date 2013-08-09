using System;
using OpenQA.Selenium;
using Selenite.Commands.Base;
using Xunit;

namespace Selenite.Commands.Implementation
{
    /// <summary>
    /// Checks to see if the specified Value is contained in the selected element's Attribute.  Requires an element to be selected.
    /// </summary>
    public class IsAttributeContainedCommand : SingleSelectorCommandBase
    {
        /// <summary>
        /// The name of the Attribute in the selected element to be checked.
        /// This parameter is required.
        /// </summary>
        public string Attribute { get; set; }

        /// <summary>
        /// The string to be compared against the selected element's attribute.
        /// This parameter is required.
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Boolean property (true/false) that can be used to make the value comparison case sensitive.
        /// Will default to false if not set (ignoring case).
        /// </summary>
        public bool IsCaseSensitive { get; set; }

        /// <summary>
        /// Boolean property (true/false) that reverses the way the command behaves.  Setting to true will return success if Value is NOT contained in the selected element's Attribute.
        /// Will default to false if not set.
        /// </summary>
        public bool IsFalseExpected { get; set; }

        protected override void Execute(IWebDriver driver, dynamic context, IWebElement element)
        {
            var stringComparison = IsCaseSensitive
                ? StringComparison.InvariantCulture
                : StringComparison.InvariantCultureIgnoreCase;

            var resolvedAttribute = Test.ResolveMacros(Attribute);
            var resolvedValue = Test.ResolveMacros(Value);

            var attribute = element
                .GetAttribute(resolvedAttribute)
                .Trim();

            if (IsFalseExpected)
                Assert.DoesNotContain(resolvedValue.Trim(), attribute, stringComparison);
            else
                Assert.Contains(resolvedValue.Trim(), attribute, stringComparison);
        }
    }
}