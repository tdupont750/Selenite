using System;
using OpenQA.Selenium;
using Selenite.Commands.Base;
using Xunit;

namespace Selenite.Commands.Implementation
{
    /// <summary>
    /// Checks the selected element's classes for the specified Class.  Requires an element to be selected.
    /// </summary>
    public class IsClassCommand : SingleSelectorCommandBase
    {
        /// <summary>
        /// The string to be compared against the selected element's classes.
        /// This parameter is required.
        /// </summary>
        public string Class { get; set; }

        /// <summary>
        /// Boolean property (true/false) that can be used to make the class comparison case sensitive.
        /// Will default to false if not set (ignoring case).
        /// </summary>
        public bool IsCaseSensitive { get; set; }

        /// <summary>
        /// Boolean property (true/false) that reverses the way the command behaves.  Setting to true will return success if the selected element is NOT have class Class.
        /// Will default to false if not set.
        /// </summary>
        public bool IsFalseExpected { get; set; }

        protected override void Execute(IWebDriver driver, dynamic contetx, IWebElement element)
        {
            var stringComparer = IsCaseSensitive
                ? StringComparer.InvariantCulture
                : StringComparer.InvariantCultureIgnoreCase;

            var classes = element.GetAttribute("class").Split(' ');
            var resolvedClass = Test.ResolveMacros(Class);

            if (IsFalseExpected)
                Assert.DoesNotContain(resolvedClass, classes, stringComparer);
            else
                Assert.Contains(resolvedClass, classes, stringComparer);
        }
    }
}