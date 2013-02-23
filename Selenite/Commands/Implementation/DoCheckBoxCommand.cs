using OpenQA.Selenium;
using Selenite.Commands.Base;
using Selenite.Enums;
using Selenite.Extensions;

namespace Selenite.Commands.Implementation
{
    public class DoCheckBoxCommand : SingleSelectorCommandBase
    {
        protected override void Execute(IWebDriver driver, dynamic context, IWebElement element)
        {
            if (!element.IsChecked())
                driver.Click(element, (DriverType)context.DriverType);
        }
    }
}