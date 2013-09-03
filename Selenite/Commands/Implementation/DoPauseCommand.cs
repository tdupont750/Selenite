using System.ComponentModel;
using System.Threading;
using OpenQA.Selenium;
using Selenite.Commands.Base;

namespace Selenite.Commands.Implementation
{
    [Description(@"Creates an artificial pause in commands to allow the webdriver/site to perform operations.  Lasts for Duration.  Does not require an element.
Does not return a result, so shouldn't affect tests.")]
    public class DoPauseCommand : CommandBase
    {
        [Description(@"The specified time to pause, in milliseconds (2000 = 2 seconds).
This parameter is required.")]
        public int Duration { get; set; }

        public override void Execute(IWebDriver driver, dynamic context)
        {
            Thread.Sleep(Duration);
        }
    }
}