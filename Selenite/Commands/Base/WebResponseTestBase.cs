using OpenQA.Selenium;
using System.Net;

namespace Selenite.Commands.Base
{
    public abstract class WebResponseTestBase : CommandBase
    {
        public override void Execute(IWebDriver driver, dynamic context)
        {
            Execute(driver, context, driver.PageSource);
        }

        protected abstract void Execute(IWebDriver driver, dynamic context, string pageSource);
    }
}
