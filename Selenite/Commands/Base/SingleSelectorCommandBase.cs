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

        protected bool AllowNullElement { get; set; }

        public override void Execute(IWebDriver driver, dynamic context)
        {
            var resolvedSelector = Test.ResolveMacros(Selector);
            IWebElement element;

            // There is a default implicit 3 second wait for "FindElements" if it can't find the element that we don't want.
            // So temporarily set it to 0.
            driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(0));

            try
            {
                if (Wait)
                {
                    var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(WaitTimeout == 0 ? 5000 : WaitTimeout));
                    element = wait.Until(d => AllowNullElement
                                                  ? d.FindElements(By.CssSelector(resolvedSelector)).Count > 0
                                                        ? d.FindElement(By.CssSelector(resolvedSelector))
                                                        : null
                                                  : d.FindElement(By.CssSelector(resolvedSelector)));
                }
                else
                {
                    element = AllowNullElement
                                  ? driver.FindElements(By.CssSelector(resolvedSelector)).Count > 0
                                        ? driver.FindElement(By.CssSelector(resolvedSelector))
                                        : null
                                  : driver.FindElement(By.CssSelector(resolvedSelector));
                }
            }
            finally
            {
                // Reset the Implicit wait to 3 seconds.
                driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(3));
            }

            Execute(driver, context, element);
        }

        protected abstract void Execute(IWebDriver driver, dynamic context, IWebElement element);
    }
}