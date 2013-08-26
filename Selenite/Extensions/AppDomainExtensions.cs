using System;
using System.Collections.Generic;
using System.Linq;

namespace Selenite
{
    public static class AppDomainExtensions
    {
        public static IList<Tuple<Type, T>> GetTypesWithAttribute<T>(this AppDomain appDomain)
            where T : class
        {
            var results = new List<Tuple<Type, T>>();

            var assemblies = appDomain.GetAssemblies()
                .Where(a => a.GetCustomAttributes(typeof (SeleniteAssemblyAttribute), false).Any())
                .ToList();

            foreach (var assembly in assemblies)
            {
                var types = assembly.GetTypes();
                foreach (var type in types)
                {
                    var attribute = type.GetCustomAttributes(typeof(T), true).FirstOrDefault() as T;
                    if (attribute != null)
                        results.Add(new Tuple<Type, T>(type, attribute));
                }
            }

            return results;
        }
    }
}