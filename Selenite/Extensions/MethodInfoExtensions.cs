using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Selenite
{
    public static class MethodInfoExtensions
    {
        public static IEnumerable<T> GetCustomAttributes<T>(this MethodInfo methodInfo, bool inherit)
        {
            return methodInfo
                .GetCustomAttributes(typeof(T), inherit)
                .Cast<T>();
        }
    }
}