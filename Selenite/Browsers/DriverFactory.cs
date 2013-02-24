using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using Selenite.Enums;
using Selenite.Global;
using Selenite.Services;

namespace Selenite.Browsers
{
    public class DriverFactory : IDisposable
    {
        private readonly IConfigurationService _configurationService = ServiceResolver.Get<IConfigurationService>();
        
        private bool _isDisposed;
        private IWebDriver _driver;
        private DriverType? _type;

        ~DriverFactory()
        {
            Dispose(true);
        }

        public void Init(DriverType browser)
        {
            if (_driver != null)
            {
                if (_type == browser)
                    return;

                throw new InvalidOperationException("DriverService already initialized");
            }

            switch (browser)
            {
                case DriverType.Firefox:
                    _driver = new FirefoxDriver();
                    break;

                case DriverType.InternetExplorer:
                    _driver = new InternetExplorerDriver(_configurationService.InternetExplorerDriverPath);
                    break;

                case DriverType.Chrome:
                    _driver = new ChromeDriver(_configurationService.ChromeDriverPath);
                    break;
            }

            _type = browser;
        }

        public IWebDriver GetBrowser()
        {
            if (_driver == null)
                throw new InvalidOperationException("DriverService not yet initialized");

            return _driver;
        }

        public void Dispose()
        {
            Dispose(false);
        }

        private void Dispose(bool isFinalizing)
        {
            if (_isDisposed)
                return;

            if (_driver != null)
            {
                if (_type == DriverType.Firefox)
                    _driver.Close();
                else
                    _driver.Quit();

                _driver.Dispose();
                _driver = null;
            }

            if (!isFinalizing)
                GC.SuppressFinalize(this);

            _isDisposed = true;
        }
    }
}