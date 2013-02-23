using OpenQA.Selenium;

namespace Selenite.Commands.Base
{
    public abstract class SingleSelectorCommandBase : CommandBase
    {
        public string Selector { get; set; }

        public override void Execute(IWebDriver driver, dynamic context)
        {
            var element = driver.FindElement(By.CssSelector(Selector));
            Execute(driver, context, element);
        }

        protected abstract void Execute(IWebDriver driver, dynamic contetx, IWebElement element);
    }
}