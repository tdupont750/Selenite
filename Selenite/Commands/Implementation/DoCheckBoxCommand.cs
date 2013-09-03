using System.ComponentModel;
using OpenQA.Selenium;
using Selenite.Commands.Base;
using Selenite.Enums;

namespace Selenite.Commands.Implementation
{
    [Description("Performs a click on the selected element, but only when the element is NOT IsChecked().  Requires an element to be selected.")]
    public class DoCheckBoxCommand : SingleSelectorCommandBase
    {
        protected override void Execute(IWebDriver driver, dynamic context, IWebElement element)
        {
            if (!element.IsChecked())
                driver.Click(element, (DriverType)context.DriverType);
        }
    }
}