using System;
using System.ComponentModel;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace Selenite.Commands.Base
{
    [Description(@"This is the base class for all commands that use a single element on the page. It will find the FIRST element that matches the Selector and perform the command on it.")]
    public abstract class SingleSelectorCommandBase : CommandBase
    {

        [Description(@"The string used to find the element on the page using standard jquery selectors.
For example, '#container input.button' will find the FIRST input element with the class 'button' inside the element with ID 'container'.")]
        public string Selector { get; set; }

        [Description("Whether or not the command should wait for the element to exist.")]
        public bool Wait { get; set; }

        [Description("The timeout period in milliseconds to wait for the element with the given selector to exist.")]
        public int WaitTimeout { get; set; }

        public override void Execute(IWebDriver driver, dynamic context)
        {
            var resolvedSelector = Test.ResolveMacros(Selector);
            IWebElement element;
            if (Wait)
            {
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(WaitTimeout == 0 ? 5000 : WaitTimeout));
                element = wait.Until(d => d.FindElement(By.CssSelector(resolvedSelector)));
            }
            else {
                element = driver.FindElement(By.CssSelector(resolvedSelector));
            }
            Execute(driver, context, element);
        }

        protected abstract void Execute(IWebDriver driver, dynamic contetx, IWebElement element);
    }
}