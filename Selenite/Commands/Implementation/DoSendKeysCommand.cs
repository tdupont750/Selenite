using System.Threading;
using OpenQA.Selenium;
using Selenite.Commands.Base;
using Selenite.Enums;

namespace Selenite.Commands.Implementation
{
    /// <summary>
    /// Sends a series of keystrokes to the selected element.  Requires an element to be selected.
    /// </summary>
    public class DoSendKeysCommand : SingleSelectorCommandBase
    {
        /// <summary>
        /// The keys to be sent to the selected element.  Each character is sent individually until the string is completed.
        /// This parameter is required.
        /// </summary>
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