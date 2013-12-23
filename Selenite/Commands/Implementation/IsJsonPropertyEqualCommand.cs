using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenQA.Selenium;
using Selenite.Commands.Base;
using Xunit;

namespace Selenite.Commands.Implementation
{
    public class IsJsonPropertyEqualCommand : WebResponseTestBase
    {
        public string PropertyName { get; set; }
        public string PropertyValue { get; set; }
        public bool IsCaseSensitive { get; set; }

        public IsJsonPropertyEqualCommand()
        {
            IsCaseSensitive = false;
        }

        protected override void Execute(IWebDriver driver, dynamic context, string pageSource)
        {
            var jsonObj = (JObject)JsonConvert.DeserializeObject(pageSource);
            var propertyValue = jsonObj.Property(PropertyName);

            var stringComparer = IsCaseSensitive
                ? StringComparer.InvariantCulture
                : StringComparer.InvariantCultureIgnoreCase;

            Assert.NotNull(propertyValue);
            Assert.Equal(PropertyValue, propertyValue.Value.ToString(), stringComparer);
        }
    }
}
