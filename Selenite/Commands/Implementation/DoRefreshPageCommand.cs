using OpenQA.Selenium;
using Selenite.Commands.Base;
using System.ComponentModel;

namespace Selenite.Commands.Implementation
{
    [Description("Refreshes the page.")]
    public class DoRefreshPageCommand : CommandBase
    {
        public override void Execute(IWebDriver driver, dynamic context)
        {
            driver.Navigate().Refresh();
        }
    }
}