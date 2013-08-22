using System;
using System.Collections.Generic;
using System.Linq;

namespace Selenite
{
    public static class TypeExtensions
    {
        internal static string GetCommandName(this Type type)
        {
            return type.Name.Replace("Command", String.Empty);
        }

        public static IEnumerable<T> GetCustomAttributes<T>(this Type type, bool inherit)
        {
            return type
                .GetCustomAttributes(typeof(T), inherit)
                .Cast<T>();
        }
    }
}