using System;
using System.Collections.ObjectModel;
using System.Net;
using OpenQA.Selenium;

namespace Selenite.Services.Implementation
{
    public class WebClientDriver : IWebDriver, ITakesScreenshot
    {
        public Screenshot GetScreenshot()
        {
            throw new NotSupportedException();
        }

        public IWebElement FindElement(By @by)
        {
            throw new NotSupportedException();
        }

        public ReadOnlyCollection<IWebElement> FindElements(By @by)
        {
            throw new NotSupportedException();
        }

        public void Dispose()
        {
            throw new NotSupportedException();
        }

        public void Close()
        {
            throw new NotSupportedException();
        }

        public void Quit()
        {
            throw new NotSupportedException();
        }

        public IOptions Manage()
        {
            throw new NotSupportedException();
        }

        public INavigation Navigate()
        {
            throw new NotSupportedException();
        }

        public ITargetLocator SwitchTo()
        {
            throw new NotSupportedException();
        }

        private string _url;
        public string Url
        {
            get { return _url; }
            set
            {
                _url = value;

                var client = new WebClient();
                PageSource = client.DownloadString(_url);

            }
        }
        public string Title { get; private set; }
        public string PageSource { get; internal set; }
        public string CurrentWindowHandle { get; private set; }
        public ReadOnlyCollection<string> WindowHandles { get; private set; }
    }
}
