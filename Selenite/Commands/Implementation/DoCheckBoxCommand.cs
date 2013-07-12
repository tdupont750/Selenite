using OpenQA.Selenium;
using Selenite.Commands.Base;
using Selenite.Enums;
using Selenite.Extensions;

namespace Selenite.Commands.Implementation
{
    /// <summary>
    /// Performs a click on the selected element, but only when the element is NOT IsChecked().  Requires an element to be selected.
    /// </summary>
    public class DoCheckBoxCommand : SingleSelectorCommandBase
    {
        protected override void Execute(IWebDriver driver, dynamic context, IWebElement element)
        {
            if (!element.IsChecked())
                driver.Click(element, (DriverType)context.DriverType);
        }
    }
}