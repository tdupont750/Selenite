using OpenQA.Selenium;

namespace Selenite.Commands.Base
{
    /// <summary>
    /// This is the base class for all commands that use a single element on the page.  It will find the FIRST element that matches the Selector and perform the command on it.
    /// </summary>
    public abstract class SingleSelectorCommandBase : CommandBase
    {
        /// <summary>
        /// The string used to find the element on the page using standard jquery selectors.
        /// For example, "#container input.button" will find the FIRST input element with the class 'button' inside the element with ID 'container'.
        /// </summary>
        public string Selector { get; set; }

        public override void Execute(IWebDriver driver, dynamic context)
        {
            var element = driver.FindElement(By.CssSelector(Selector));
            Execute(driver, context, element);
        }

        protected abstract void Execute(IWebDriver driver, dynamic contetx, IWebElement element);
    }
}