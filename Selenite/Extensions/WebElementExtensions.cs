using System;
using System.Linq;
using OpenQA.Selenium;

namespace Selenite
{
    public static class WebElementExtensions
    {
        private static readonly StringComparer DefaultComparer = StringComparer.InvariantCultureIgnoreCase;

        private static readonly string[] Selected = new[] {"true", "selected"};

        public static bool IsSelected(this IWebElement element)
        {
            var attribute = element.GetAttribute("selected");
            return Selected.Contains(attribute, DefaultComparer);
        }

        private static readonly string[] Checked = new[] {"true", "checked"};

        public static bool IsChecked(this IWebElement element)
        {
            var attribute = element.GetAttribute("checked");
            return Checked.Contains(attribute, DefaultComparer);
        }

        private const StringComparison DefaultComparison = StringComparison.InvariantCultureIgnoreCase;

        public static IWebElement GetOptionByValue(this IWebElement element, string value, StringComparison? stringComparison)
        {
            return element.GetOption(o => value.Equals(o.GetAttribute("value"), stringComparison ?? DefaultComparison));
        }

        public static IWebElement GetOptionByText(this IWebElement element, string text, StringComparison? stringComparison)
        {
            return element.GetOption(o => text.Equals(o.Text, stringComparison ?? DefaultComparison));
        }

        private static IWebElement GetOption(this IWebElement element, Func<IWebElement, bool> predicate)
        {
            return element
                .FindElements(By.CssSelector("option"))
                .FirstOrDefault(predicate);
        }
    }
}