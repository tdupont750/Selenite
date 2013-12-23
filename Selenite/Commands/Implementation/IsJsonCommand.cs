using Newtonsoft.Json;
using OpenQA.Selenium;
using Selenite.Commands.Base;
using Xunit;

namespace Selenite.Commands.Implementation
{
    public class IsJsonCommand : WebResponseTestBase
    {
        protected override void Execute(IWebDriver driver, dynamic context, string pageSource)
        {
            JsonConvert.DeserializeObject(pageSource);
        }
    }
}
