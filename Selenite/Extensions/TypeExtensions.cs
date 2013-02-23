using System;

namespace Selenite.Extensions
{
    public static class TypeExtensions
    {
        internal static string GetCommandName(this Type type)
        {
            return type.Name.Replace("Command", String.Empty);
        }
    }
}