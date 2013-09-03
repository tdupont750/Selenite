using System.ComponentModel;
using OpenQA.Selenium;
using Selenite.Commands.Base;
using Selenite.Enums;

namespace Selenite.Commands.Implementation
{
    [Description("Performs a click on the selected element.  Requires an element to be selected.")]
    public class DoClickCommand : SingleSelectorCommandBase
    {
        protected override void Execute(IWebDriver driver, dynamic context, IWebElement element)
        {
            driver.Click(element, (DriverType)context.DriverType);
        }
    }
}