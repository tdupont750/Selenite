using System.Threading;
using OpenQA.Selenium;
using Selenite.Commands.Base;
using Selenite.Enums;

namespace Selenite.Commands.Implementation
{
    public class DoSendKeysCommand : SingleSelectorCommandBase
    {
        public string Keys { get; set; }

        protected override void Execute(IWebDriver driver, dynamic context, IWebElement element)
        {
            // HACK: Firefox has issues with typing long strings.
            if (context.DriverType == DriverType.Firefox)
            {
                foreach (var key in Keys)
                {
                    Thread.Sleep(50);
                    element.SendKeys(key.ToString());
                }
            }
            else
            {
                element.SendKeys(Keys);
            }
        }
    }
}