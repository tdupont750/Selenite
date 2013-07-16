using System.Threading;
using OpenQA.Selenium;
using Selenite.Commands.Base;

namespace Selenite.Commands.Implementation
{
    /// <summary>
    /// Creates an artificial pause in commands to allow the webdriver/site to perform operations.  Lasts for Duration.  Does not require an element.
    /// Does not return a result, so shouldn't affect tests.
    /// </summary>
    public class DoPauseCommand : CommandBase
    {
        /// <summary>
        /// The specified time to pause, in milliseconds (2000 = 2 seconds).
        /// This parameter is required.
        /// </summary>
        public int Duration { get; set; }

        public override void Execute(IWebDriver driver, dynamic context)
        {
            Thread.Sleep(Duration);
        }
    }
}