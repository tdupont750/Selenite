using OpenQA.Selenium;
using Selenite.Commands.Base;

namespace Selenite.Commands.Implementation
{
    /// <summary>
    /// Clears the text within the selected element.  Requires an element to be selected.
    /// </summary>
    public class DoClearCommand : SingleSelectorCommandBase
    {
        protected override void Execute(IWebDriver driver, dynamic context, IWebElement element)
        {
            element.Clear();
        }
    }
}