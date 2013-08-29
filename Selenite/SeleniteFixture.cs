using System;
using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;
using Selenite.Enums;
using Selenite.Global;
using Selenite.Models;
using Selenite.Services;

namespace Selenite
{
    public class SeleniteFixture : IDisposable
    {
        private readonly ITestService _testService = ServiceResolver.Get<ITestService>();
        private readonly IDriverFactory _driverFactory = ServiceResolver.Get<IDriverFactory>();
        private readonly IDictionary<WeakReference, IList<string>> _setupStepsMap = new Dictionary<WeakReference, IList<string>>();
        
        private bool _isDisposed;

        ~SeleniteFixture()
        {
            Dispose(false);
        }

        public void ExecuteTest(DriverType driverType, SeleniteTest test)
        {
            var webDriver = _driverFactory.GetBrowser(driverType);

            TrySetupSteps(driverType, test, webDriver);

            _testService.ExecuteTest(webDriver, driverType, test);
        }

        private void TrySetupSteps(DriverType driverType, SeleniteTest test, IWebDriver webDriver)
        {
            if (test.TestCollection.SetupSteps == null)
                return;

            var fileName = String.IsNullOrWhiteSpace(test.TestCollection.SetupStepsFile)
                ? test.TestCollection.ResolvedFile
                : test.TestCollection.SetupStepsFile;

            foreach (var pair in _setupStepsMap)
            {
                if (pair.Key.Target != webDriver)
                    continue;

                if (pair.Value.Any(f => String.Equals(f, fileName, StringComparison.InvariantCultureIgnoreCase)))
                    return;
            }

            foreach (var setupStep in test.TestCollection.SetupSteps)
                _testService.ExecuteTest(webDriver, driverType, setupStep, true);

            var weakReference = new WeakReference(webDriver);
            var testFiles = new List<string> { fileName };
            _setupStepsMap.Add(weakReference, testFiles);
        }

        public void Dispose()
        {
            Dispose(true);
        }

        private void Dispose(bool isDisposing)
        {
            if (_isDisposed)
                return;

            _driverFactory.Dispose();

            if (isDisposing)
                GC.SuppressFinalize(this);

            _isDisposed = true;
        }
    }
}
