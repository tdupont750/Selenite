using System;
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

        private bool _isDisposed;

        ~SeleniteFixture()
        {
            Dispose(false);
        }

        public void ExecuteTest(DriverType driverType, SeleniteTest test)
        {
            var webDriver = _driverFactory.GetBrowser(driverType);
            _testService.ExecuteTest(webDriver, driverType, test);
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
