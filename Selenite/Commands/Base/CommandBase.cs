using OpenQA.Selenium;
using Selenite.Extensions;

namespace Selenite.Commands.Base
{
    [Command]
    public abstract class CommandBase : ICommand
    {
        public string Name
        {
            get { return GetType().GetCommandName(); }
        }

        public virtual void Validate()
        {
        }

        public abstract void Execute(IWebDriver driver, dynamic context);
    }
}
