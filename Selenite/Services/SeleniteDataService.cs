using System;
using System.Collections.Generic;
using System.Reflection;

namespace Selenite.Services
{
    public interface ISeleniteDataService
    {
        IEnumerable<object[]> GetData(MethodInfo methodUnderTest, Type[] parameterTypes);
    }
}
