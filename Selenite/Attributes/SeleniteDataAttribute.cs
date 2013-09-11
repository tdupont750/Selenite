using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Selenite.Enums;
using Selenite.Global;
using Selenite.Models;
using Selenite.Services;
using Xunit.Extensions;

namespace Selenite
{
    public class SeleniteDataAttribute : DataAttribute
    {
        private static readonly ISeleniteDataService SeleniteDataService = ServiceResolver.Get<ISeleniteDataService>();

        public override IEnumerable<object[]> GetData(MethodInfo methodUnderTest, Type[] parameterTypes)
        {
            return SeleniteDataService.GetData(methodUnderTest, parameterTypes);
        }
    }
}