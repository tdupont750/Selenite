using OpenQA.Selenium;
using Selenite.Models;
using Newtonsoft.Json;

namespace Selenite.Commands.Base
{
    [Command]
    public abstract class CommandBase : ICommand
    {
        [JsonIgnore]
        public SeleniteTest Test { get; set; }

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
