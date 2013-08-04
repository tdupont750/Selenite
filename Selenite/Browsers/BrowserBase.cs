using System;
using OpenQA.Selenium;
using Selenite.Enums;
using Selenite.Global;
using Selenite.Models;
using Selenite.Services;
using Xunit;

namespace Selenite.Browsers
{
    public abstract class BrowserBase : IDisposable, IUseFixture<DriverFactory>
    {
        protected const string AboutBlank = "about:blank";

        protected static readonly ITestService TestService = ServiceResolver.Get<ITestService>();

        private bool _isDisposed;

        public IWebDriver Driver { get; private set; }

        public abstract DriverType DriverType { get; }
        
        ~BrowserBase()
        {
            Dispose(true);
        }

        public void SetFixture(DriverFactory driverFactory)
        {
            driverFactory.Init(DriverType);
            Driver = driverFactory.GetBrowser();
        }

        protected void ExecuteTest(Test test)
        {
            TestService.ExecuteTest(this, test);
        }

        public void Dispose()
        {
            Dispose(false);
        }

        private void Dispose(bool isFinalizing)
        {
            if (_isDisposed)
                return;

            if (Driver != null)
                Driver.Url = AboutBlank;

            if (!isFinalizing)
                GC.SuppressFinalize(this);

            _isDisposed = true;
        }
    }
}