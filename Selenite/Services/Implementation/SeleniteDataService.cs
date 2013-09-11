using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Selenite.Enums;
using Selenite.Models;

namespace Selenite.Services.Implementation
{
    public class SeleniteDataService : ISeleniteDataService
    {
        private static readonly IList<DriverType> DriverTypeValues = Enum
            .GetValues(typeof(DriverType))
            .Cast<DriverType>()
            .Where(v => v != DriverType.Unknown)
            .ToArray();

        private readonly ITestCollectionService _testCollectionService;
        private readonly IManifestService _manifestService;

        public SeleniteDataService(ITestCollectionService testCollectionService, IManifestService manifestService)
        {
            _testCollectionService = testCollectionService;
            _manifestService = manifestService;
        }

        public IEnumerable<object[]> GetData(MethodInfo methodUnderTest, Type[] parameterTypes)
        {
            var overrideDomain = GetDomainOverride(methodUnderTest);

            IList<DriverType> manifestDriverTypes;
            var testCollections = GetTestCollectionFiles(methodUnderTest, overrideDomain, out manifestDriverTypes);

            var driverTypes = GetDriverTypes(methodUnderTest);
            var tests = GetTests(methodUnderTest, testCollections, driverTypes, manifestDriverTypes);

            foreach (var driverType in DriverTypeValues)
                foreach (var test in tests)
                    if (test.Item1.Contains(driverType))
                        yield return new object[] { driverType, test.Item2 };
        }

        private IList<TestCollection> GetTestCollectionFiles(
            MethodInfo methodUnderTest,
            string overrideDomain,
            out IList<DriverType> manifestDriverTypes)
        {
            var testCollectionFiles = GetAttributeValues<SeleniteTestCollectionAttribute, string>(methodUnderTest, a => a.Collections);

            if (testCollectionFiles.Any())
            {
                manifestDriverTypes = null;
                return _testCollectionService.GetTestCollections(testCollectionFiles, overrideDomain);
            }

            var manifestName = _manifestService.GetActiveManifestName();
            var manifest = _manifestService.GetManifest(manifestName);
            manifestDriverTypes = manifest.DriverTypes;
            return _testCollectionService.GetTestCollections(manifest, overrideDomain);
        }

        private static IList<Tuple<IList<DriverType>, SeleniteTest>> GetTests(
            MethodInfo methodUnderTest,
            IList<TestCollection> testCollections,
            IList<DriverType> driverTypes,
            IList<DriverType> manifestDriverTypes)
        {
            var results = new List<Tuple<IList<DriverType>, SeleniteTest>>();
            var testNames = GetAttributeValues<SeleniteTestAttribute, string>(methodUnderTest, a => a.Tests);

            // TODO Add reflection option.

            // ReSharper disable PossibleMultipleEnumeration
            // ReSharper disable LoopCanBeConvertedToQuery
            if (testNames.Any())
            {
                foreach (var testCollection in testCollections)
                {
                    var testCollectionDriverTypes = Intersections(true, driverTypes, manifestDriverTypes, testCollection.DriverTypes);
                    foreach (var test in testCollection.Tests)
                    {
                        if (!testNames.Any(n => n.Equals(test.Name, StringComparison.InvariantCultureIgnoreCase)))
                            continue;

                        var testDriverTypes = Intersections(false, testCollectionDriverTypes, test.DriverTypes);
                        if (testDriverTypes.IsNullOrNotAny())
                            continue;

                        var result = new Tuple<IList<DriverType>, SeleniteTest>(testDriverTypes.ToArray(), test);
                        results.Add(result);
                    }
                }
            }
            else
            {
                foreach (var testCollection in testCollections)
                {
                    if (!testCollection.Enabled)
                        continue;

                    var testCollectionDriverTypes = Intersections(true, driverTypes, manifestDriverTypes, testCollection.DriverTypes);
                    foreach (var test in testCollection.Tests)
                    {
                        if (!test.Enabled)
                            continue;

                        var testDriverTypes = Intersections(false, testCollectionDriverTypes, test.DriverTypes);
                        if (testDriverTypes.IsNullOrNotAny())
                            continue;

                        var result = new Tuple<IList<DriverType>, SeleniteTest>(testDriverTypes.ToArray(), test);
                        results.Add(result);
                    }
                }
            }
            // ReSharper restore LoopCanBeConvertedToQuery
            // ReSharper restore PossibleMultipleEnumeration

            return results;
        }

        private static IList<DriverType> GetDriverTypes(MethodInfo methodUnderTest)
        {
            var driverTypes = GetAttributeValues<SeleniteDriverAttribute, DriverType>(methodUnderTest, a => a.DriverTypes);

            if (driverTypes.Any())
                return driverTypes;

            return DriverTypeValues;
        }

        private static string GetDomainOverride(MethodInfo methodUnderTest)
        {
            var domainOverrides = GetAttributeValues<SeleniteDomainOverrideAttribute, string>(methodUnderTest, a => new[] { a.DomainOverride });
            return domainOverrides.FirstOrDefault(o => !String.IsNullOrWhiteSpace(o));
        }

        private static IList<TR> GetAttributeValues<T, TR>(MethodInfo methodUnderTest, Func<T, IEnumerable<TR>> selectMany)
        {
            var methodResults = methodUnderTest
                .GetCustomAttributes<T>(true)
                .SelectMany(selectMany)
                .ToArray();

            if (methodResults.Any())
                return methodResults;

            return methodUnderTest.DeclaringType
                .GetCustomAttributes<T>(true)
                .SelectMany(selectMany)
                .ToArray();
        }

        private static IEnumerable<T> Intersections<T>(bool firstAllowNull, params IEnumerable<T>[] collections)
        {
            if (!firstAllowNull && !collections.IsNullOrNotAny())
            {
                var first = collections.First();
                if (first != null && !first.Any())
                    return Enumerable.Empty<T>();
            }

            IEnumerable<T> results = null;

            // ReSharper disable PossibleMultipleEnumeration
            // ReSharper disable LoopCanBeConvertedToQuery
            foreach (var collection in collections)
            {
                if (collection.IsNullOrNotAny())
                    continue;

                results = results == null
                    ? collection.ToList()
                    : results.Intersect(collection);
            }
            // ReSharper restore LoopCanBeConvertedToQuery
            // ReSharper restore PossibleMultipleEnumeration

            return results;
        }
    }
}
