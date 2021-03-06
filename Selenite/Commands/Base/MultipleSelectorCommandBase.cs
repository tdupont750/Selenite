using System.Collections.Generic;
using System.ComponentModel;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;

namespace Selenite.Commands.Base
{
    [Description("This is the base class for all commands that use multiple elements on the page.  It will find ALL elements that match the Selector and perform the command on them.")]
    public abstract class MultipleSelectorCommandBase : CommandBase
    {
        [Description(@"The string used to find the element on the page using standard jquery selectors.
For example, '#container input.button' will find the ALL input elements with the class 'button' inside the element with ID 'container'.")]
        public string Selector { get; set; }

        [Description("Whether or not the command should wait for the element to exist.")]
        public bool Wait { get; set; }

        [Description("The timeout period in milliseconds to wait for the element with the given selector to exist.")]
        public int WaitTimeout { get; set; }

        public override void Execute(IWebDriver driver, dynamic context)
        {
            var resolvedSelector = Test.ResolveMacros(Selector);
            IList<IWebElement> elements;
            if (Wait)
            {
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(WaitTimeout == 0 ? 5000 : WaitTimeout));
                var element = wait.Until(d => d.FindElement(By.CssSelector(resolvedSelector)));
            }

            elements = driver.FindElements(By.CssSelector(resolvedSelector));
            Execute(driver, context, elements);
        }

        protected abstract void Execute(IWebDriver driver, dynamic context, IList<IWebElement> elements);
    }
}