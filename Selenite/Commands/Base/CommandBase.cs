using OpenQA.Selenium;
using Selenite.Extensions;
using Selenite.Models;
using Newtonsoft.Json;

namespace Selenite.Commands.Base
{
    [Command]
    public abstract class CommandBase : ICommand
    {
        [JsonIgnore]
        public Test Test { get; set; }

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
