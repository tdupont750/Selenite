using System.Collections.Generic;
using OpenQA.Selenium;

namespace Selenite.Commands.Base
{
    public abstract class MultipleSelectorCommandBase : CommandBase
    {
        public string Selector { get; set; }

        public override void Execute(IWebDriver driver, dynamic context)
        {
            var elements = driver.FindElements(By.CssSelector(Selector));
            Execute(driver, context, elements);
        }

        protected abstract void Execute(IWebDriver driver, dynamic context, IList<IWebElement> elements);
    }
}