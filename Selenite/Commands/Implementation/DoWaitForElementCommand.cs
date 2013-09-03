using System;
using System.ComponentModel;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Selenite.Commands.Base;

namespace Selenite.Commands.Implementation
{
    [Description("Waits for a given element to exist for up to the given timeout period.")]
    public class DoWaitForElementCommand : CommandBase
    {
        [Description(@"The specified time to pause, in milliseconds (2000 = 2 seconds).
This parameter is required.")]
        public int Timeout { get; set; }

        [Description("The selector for the element to wait for.")]
        public string Selector { get; set; }

        [Description("If true, waits for the element to be visible as well.")]
        public bool WaitForVisible { get; set; }

        [Description("If true, the element is expected not to exist after the timeout period.")]
        public bool IsFalseExpected { get; set; }

        public override void Execute(IWebDriver driver, dynamic context)
        {
            var resolvedSelector = Test.ResolveMacros(Selector);

            var wait = new WebDriverWait(driver, TimeSpan.FromMilliseconds(Timeout == 0 ? 5000 : Timeout));

            try
            {
                if (WaitForVisible)
                {
                    wait.Until(d => ExpectedConditions.ElementIsVisible(By.CssSelector(resolvedSelector))(d));
                }
                else
                {
                    wait.Until(d => d.FindElement(By.CssSelector(resolvedSelector)));
                }
                if (IsFalseExpected)
                    throw new InvalidOperationException("Element with selector '" + Selector + "' exists.");
            }
            catch (WebDriverTimeoutException)
            {
                if (!IsFalseExpected)
                    throw;
            }
        }
    }
}