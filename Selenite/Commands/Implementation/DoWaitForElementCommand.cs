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
        public DoWaitForElementCommand()
        {
            Timeout = 5000;
        }
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
        /// If true, the element is expected not to exist after the timeout period.
        /// </summary>
        public bool IsFalseExpected { get; set; }

        public override void Execute(IWebDriver driver, dynamic context)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromMilliseconds(Timeout));
            
            try {
                wait.Until(d => d.FindElement(By.CssSelector(Selector)));
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