using OpenQA.Selenium;
using Selenite.Enums;
using Selenite.Models;

namespace Selenite.Services
{
    public interface ITestService
    {
        void ExecuteTest(IWebDriver webDriver, DriverType driverType, SeleniteTest test, bool isSetup = false);
    }
}