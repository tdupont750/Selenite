using System;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Linq;

namespace Selenite.Extensions
{
    public static class EnumExtensions
    {
        private static readonly ConcurrentDictionary<Tuple<bool, Type, Type, Enum>, Attribute> AttributeMap = new ConcurrentDictionary<Tuple<bool, Type, Type, Enum>, Attribute>();
        
        /// <summary>
        /// Returns a description of the enum if provided via the Description attribute.
        /// If not provided, the normal name is returned
        /// </summary>
        public static string Description(this Enum value)
        {
            var attributes = (DescriptionAttribute[])value
                .GetType()
                .GetField(value.ToString())
                .GetCustomAttributes(typeof(DescriptionAttribute), false);

            return attributes.Length > 0 ? attributes[0].Description : value.ToString();
        }
    }
}
