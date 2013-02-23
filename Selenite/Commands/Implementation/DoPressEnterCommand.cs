using OpenQA.Selenium;
using Selenite.Commands.Base;

namespace Selenite.Commands.Implementation
{
    public class DoPressEnterCommand : SingleSelectorCommandBase
    {
        protected override void Execute(IWebDriver driver, dynamic context, IWebElement element)
        {
            element.SendKeys(Keys.Enter);
        }
    }
}