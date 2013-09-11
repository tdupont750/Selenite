using System.Collections.Generic;
using System.Linq;

namespace Selenite
{
    public static class EnumerableExtensions
    {
        public static bool IsNullOrNotAny<T>(this IEnumerable<T> enumerable)
        {
            return enumerable == null || !enumerable.Any();
        }
    }
}
