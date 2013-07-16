using OpenQA.Selenium;
using Selenite.Commands.Base;
using Selenite.Enums;
using Selenite.Extensions;

namespace Selenite.Commands.Implementation
{
    /// <summary>
    /// Performs a click on the selected element.  Requires an element to be selected.
    /// </summary>
    public class DoClickCommand : SingleSelectorCommandBase
    {
        protected override void Execute(IWebDriver driver, dynamic context, IWebElement element)
        {
            driver.Click(element, (DriverType) context.DriverType);
        }
    }
}