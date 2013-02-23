using OpenQA.Selenium;

namespace Selenite.Commands
{
    public interface ICommand
    {
        string Name { get; }

        void Validate();

        void Execute(IWebDriver driver, dynamic context);
    }
}