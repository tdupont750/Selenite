using System.ComponentModel;
using OpenQA.Selenium;
using Selenite.Commands.Base;

namespace Selenite.Commands.Implementation
{
    [Description("Sends the enter key to the selected element, as though the user pressed enter with that element as the focus.  Requires an element to be selected.")]
    public class DoPressEnterCommand : SingleSelectorCommandBase
    {
        protected override void Execute(IWebDriver driver, dynamic context, IWebElement element)
        {
            element.SendKeys(Keys.Enter);
        }
    }
}