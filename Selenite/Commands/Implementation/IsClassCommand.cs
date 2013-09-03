using System;
using System.ComponentModel;
using OpenQA.Selenium;
using Selenite.Commands.Base;
using Xunit;

namespace Selenite.Commands.Implementation
{
    [Description(@"Checks the selected element's classes for the specified Class.  Requires an element to be selected.")]
    public class IsClassCommand : SingleSelectorCommandBase
    {
        [Description(@"The string to be compared against the selected element's classes.
This parameter is required.")]
        public string Class { get; set; }

        [Description(@"Boolean property (true/false) that can be used to make the class comparison case sensitive.
Will default to false if not set (ignoring case).")]
        public bool IsCaseSensitive { get; set; }

        [Description(@"Boolean property (true/false) that reverses the way the command behaves.  Setting to true will return success if the selected element is NOT have class Class.
Will default to false if not set.")]
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