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
        private static readonly ITestCollectionService TestCollectionService = ServiceResolver.Get<ITestCollectionService>();
        private static readonly IManifestService ManifestService = ServiceResolver.Get<IManifestService>();

        public override IEnumerable<object[]> GetData(MethodInfo methodUnderTest, Type[] parameterTypes)
        {
            var overrideDomain = GetDomainOverride(methodUnderTest);
            var testCollections = GetTestCollectionFiles(methodUnderTest, overrideDomain);
            var tests = GetTests(methodUnderTest, testCollections);
            var driverTypes = GetDriverTypes(methodUnderTest);

            foreach (var driverType in driverTypes)
                foreach (var test in tests)
                    yield return new object[] {driverType, test};
        }

        private IList<TestCollection> GetTestCollectionFiles(MethodInfo methodUnderTest, string overrideDomain)
        {
            var testCollectionFiles = GetAttributeValues<SeleniteTestCollectionAttribute, string>(methodUnderTest, a => a.Collections);

            if (testCollectionFiles.Any())
                return TestCollectionService.GetTestCollections(testCollectionFiles, overrideDomain);

            var manifestName = ManifestService.GetActiveManifestName();
            var manifest = ManifestService.GetManifest(manifestName);
            return TestCollectionService.GetTestCollections(manifest, overrideDomain);
        }

        private static IList<SeleniteTest> GetTests(MethodInfo methodUnderTest, IList<TestCollection> testCollections)
        {
            var testNames = GetAttributeValues<SeleniteTestAttribute, string>(methodUnderTest, a => a.Tests);

            if (testNames.Any())
                return testCollections
                    .SelectMany(c => c.Tests)
                    .Where(t => testNames.Any(n => n.Equals(t.Name, StringComparison.InvariantCultureIgnoreCase)))
                    .ToArray();
            
            return testCollections
                .Where(tc => tc.Enabled)
                .SelectMany(c => c.Tests)
                .Where(test => test.Enabled)
                .ToArray();
        }

        private static IList<DriverType> GetDriverTypes(MethodInfo methodUnderTest)
        {
            var driverTypes = GetAttributeValues<SeleniteDriverAttribute, DriverType>(methodUnderTest, a => a.DriverTypes);

            if (driverTypes.Any())
                return driverTypes;

            return Enum
                .GetValues(typeof (DriverType))
                .Cast<DriverType>()
                .Where(v => v != DriverType.Unknown)
                .ToArray();
        }

        private static string GetDomainOverride(MethodInfo methodUnderTest)
        {
            var domainOverrides = GetAttributeValues<SeleniteDomainOverrideAttribute, string>(methodUnderTest, a => new [] { a.DomainOverride });
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
    }
}