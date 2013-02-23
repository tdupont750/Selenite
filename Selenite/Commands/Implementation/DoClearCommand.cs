using OpenQA.Selenium;
using Selenite.Commands.Base;

namespace Selenite.Commands.Implementation
{
    public class DoClearCommand : SingleSelectorCommandBase
    {
        protected override void Execute(IWebDriver driver, dynamic context, IWebElement element)
        {
            element.Clear();
        }
    }
}