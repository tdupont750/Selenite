using System.ComponentModel;
using System.Threading;
using OpenQA.Selenium;
using Selenite.Commands.Base;
using Selenite.Enums;

namespace Selenite.Commands.Implementation
{
    [Description("Sends a series of keystrokes to the selected element.  Requires an element to be selected.")]
    public class DoSendKeysCommand : SingleSelectorCommandBase
    {
        [Description(@"The keys to be sent to the selected element.  Each character is sent individually until the string is completed.
This parameter is required.")]
        public string Keys { get; set; }

        protected override void Execute(IWebDriver driver, dynamic context, IWebElement element)
        {
            var resolvedKeys = Test.ResolveMacros(Keys);

            // HACK: Firefox has issues with typing long strings.
            if (context.DriverType == DriverType.Firefox)
            {
                foreach (var key in resolvedKeys)
                {
                    Thread.Sleep(50);
                    element.SendKeys(key.ToString());
                }
            }
            else
            {
                element.SendKeys(resolvedKeys);
            }
        }
    }
}