using System.Collections.Generic;
using OpenQA.Selenium;

namespace Selenite.Commands.Base
{
    /// <summary>
    /// This is the base class for all commands that use multiple elements on the page.  It will find ALL elements that match the Selector and perform the command on them.
    /// </summary>
    public abstract class MultipleSelectorCommandBase : CommandBase
    {
        /// <summary>
        /// The string used to find the element on the page using standard jquery selectors.
        /// For example, "#container input.button" will find the ALL input elements with the class 'button' inside the element with ID 'container'.
        /// </summary>
        public string Selector { get; set; }

        public override void Execute(IWebDriver driver, dynamic context)
        {
            var elements = driver.FindElements(By.CssSelector(Selector));
            Execute(driver, context, elements);
        }

        protected abstract void Execute(IWebDriver driver, dynamic context, IList<IWebElement> elements);
    }
}