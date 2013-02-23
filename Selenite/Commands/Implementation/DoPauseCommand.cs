using System.Threading;
using OpenQA.Selenium;
using Selenite.Commands.Base;

namespace Selenite.Commands.Implementation
{
    public class DoPauseCommand : CommandBase
    {
        public int Duration { get; set; }

        public override void Execute(IWebDriver driver, dynamic context)
        {
            Thread.Sleep(Duration);
        }
    }
}