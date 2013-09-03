using System.ComponentModel;
using OpenQA.Selenium;
using Selenite.Commands.Base;
using Selenite.Enums;

namespace Selenite.Commands.Implementation
{
    [Description("Performs a click on the selected element, but only if the element IsChecked().  Requires an element to be selected.")]
    public class DoUncheckBoxCommand : SingleSelectorCommandBase
    {
        protected override void Execute(IWebDriver driver, dynamic context, IWebElement element)
        {
            if (element.IsChecked())
                driver.Click(element, (DriverType)context.DriverType);
        }
    }
}