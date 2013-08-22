using System;
using OpenQA.Selenium;
using Selenite.Enums;

namespace Selenite.Services
{
    public interface IDriverFactory : IDisposable
    {
        IWebDriver GetBrowser(DriverType browser);
    }
}
