using OpenQA.Selenium;
using Selenite.Models;

namespace Selenite.Commands
{
    public interface ICommand
    {
        SeleniteTest Test { get; set; }

        string Name { get; }

        void Validate();

        void Execute(IWebDriver driver, dynamic context);
    }
}