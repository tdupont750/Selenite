using System;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Selenite.Commands.Base;

namespace Selenite.Commands.Implementation
{
    /// <summary>
    /// Waits for a given element to exist for up to the given timeout period.
    /// </summary>
    public class DoWaitForElementCommand : CommandBase
    {
        /// <summary>
        /// The specified time to pause, in milliseconds (2000 = 2 seconds).
        /// This parameter is required.
        /// </summary>
        public int Timeout { get; set; }

        /// <summary>
        /// The selector for the element to wait for.
        /// </summary>
        public string Selector { get; set; }

        /// <summary>
        /// If true, waits for the element to be visible as well.
        /// </summary>
        public bool WaitForVisible { get; set; }

        /// <summary>
        /// If true, the element is expected not to exist after the timeout period.
        /// </summary>
        public bool IsFalseExpected { get; set; }

        public override void Execute(IWebDriver driver, dynamic context)
        {
            var resolvedSelector = Test.ResolveMacros(Selector);

            var wait = new WebDriverWait(driver, TimeSpan.FromMilliseconds(Timeout == 0 ? 5000 : Timeout));
            
            try {
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
            catch (WebDriverTimeoutException) {
                if (!IsFalseExpected)
                    throw;
            }
        }
    }
}