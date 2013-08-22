using System;
using OpenQA.Selenium;
using Selenite.Enums;

namespace Selenite
{
    public static class WebDriverExtensions
    {
        public static void Click(this IWebDriver driver, IWebElement element, DriverType driverType)
        {
            // HACK: Force Focus, to help prevent a problem with clicking in IE
            if (driverType == DriverType.InternetExplorer)
                driver.SwitchTo().Window(driver.CurrentWindowHandle);

            // HACK: There is an issue where mouse over events occure on Click that cover the buttons and cause a miss click.
            if (driverType == DriverType.Firefox && "input".Equals(element.TagName, StringComparison.InvariantCultureIgnoreCase))
                element.SendKeys(Keys.Space);
            else
                element.Click();
        }
    }
}