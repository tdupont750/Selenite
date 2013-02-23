using System;
using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;
using Selenite.Enums;
using Selenite.Global;
using Selenite.Models;
using Selenite.Services;
using Xunit;

namespace Selenite.Browsers.Base
{
    public abstract class BrowserBase : IDisposable, IUseFixture<DriverFactory>
    {
        protected const string AboutBlank = "about:blank";
        protected const string TestDataMember = "TestData";

        protected static readonly IConfigurationService ConfigurationService = ServiceResolver.Get<IConfigurationService>();
        protected static readonly ICategoryService CategoryService = ServiceResolver.Get<ICategoryService>();
        protected static readonly IManifestService ManifestService = ServiceResolver.Get<IManifestService>();
        protected static readonly ITestService TestService = ServiceResolver.Get<ITestService>();

        private bool _isDisposed;

        public IWebDriver Driver { get; private set; }

        public abstract DriverType DriverType { get; }
        
        ~BrowserBase()
        {
            Dispose(true);
        }

        public void SetFixture(DriverFactory driverService)
        {
            driverService.Init(DriverType);
            Driver = driverService.GetBrowser();
        }

        protected void ExecuteTest(Test test)
        {
            TestService.ExecuteTest(this, test);
        }

        public void Dispose()
        {
            Dispose(true);
        }

        private void Dispose(bool isDisposing)
        {
            if (_isDisposed)
                return;

            if (Driver != null)
                Driver.Url = AboutBlank;

            if (!isDisposing)
                GC.SuppressFinalize(this);

            _isDisposed = true;
        }

        public static IEnumerable<object[]> TestData
        {
            get
            {
                var manifestName = ConfigurationService.ActiveManifest;
                var manifest = ManifestService.GetManifest(manifestName);
                var categories = CategoryService.GetCategories(manifest);

                return categories
                    .SelectMany(c => c.Tests)
                    .Select(t => new object[] { t });
            }
        }
    }
}