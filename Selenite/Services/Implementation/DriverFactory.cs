using System;
using System.IO;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.PhantomJS;
using Selenite.Enums;
using Selenite.Global;

namespace Selenite.Services.Implementation
{
    public class DriverFactory : IDriverFactory
    {
        private readonly IConfigurationService _configurationService = ServiceResolver.Get<IConfigurationService>(); 
        
        private bool _isDisposed;
        private IWebDriver _driver;
        private DriverType? _type;

        ~DriverFactory()
        {
            Dispose(false);
        }

        public IWebDriver GetBrowser(DriverType browser)
        {
            if (_driver != null)
            {
                if (_type == browser)
                    return _driver;

                DisposeDriver();
            }

            _driver = CreateDriver(browser);
            _type = browser;

            var window = _driver.Manage().Window;
            window.Size = new System.Drawing.Size(1024, 800);
            window.Position = new System.Drawing.Point(0, 0);
            
            _driver.Url = Constants.AboutBlank;

            return _driver;
        }

        private IWebDriver CreateDriver(DriverType browser)
        {
            switch (browser)
            {
                case DriverType.Firefox:
                    return new FirefoxDriver();

                case DriverType.InternetExplorer:
                    return new InternetExplorerDriver(_configurationService.GetDriverPath(browser));

                case DriverType.Chrome:
                    return new ChromeDriver(_configurationService.GetDriverPath(browser));

                case DriverType.PhantomJs:
                    var service = PhantomJSDriverService.CreateDefaultService(_configurationService.GetDriverPath(browser));
                    service.CookiesFile = "cookies.txt";

                    return new PhantomJSDriver(service, new PhantomJSOptions());

                case DriverType.WebClient:
                    return new WebClientDriver();

                default:
                    throw new InvalidOperationException("Unkown Driver Type: " + browser);
            }
        }
        
        public void Dispose()
        {
            Dispose(true);
        }

        private void Dispose(bool isDisposing)
        {
            if (_isDisposed)
                return;

            DisposeDriver();

            if (isDisposing)
                GC.SuppressFinalize(this);

            _isDisposed = true;
        }

        private void DisposeDriver()
        {
            if (_driver == null) 
                return;

            if (_type == DriverType.Firefox)
                _driver.Close();
            else
                _driver.Quit();

            _driver.Dispose();
            _driver = null;
        }
    }
}