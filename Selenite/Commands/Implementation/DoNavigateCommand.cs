using OpenQA.Selenium;
using Selenite.Commands.Base;
using System.ComponentModel;

namespace Selenite.Commands.Implementation
{
    [Description("Navigates to the specified URL.")]
    public class DoNavigateCommand : CommandBase
    {
        [Description("The fully qualified URL to navigate to.")]
        public string Url { get; set; }

        public override void Execute(IWebDriver driver, dynamic context)
        {
            driver.Navigate().GoToUrl(Url);
        }
    }
}