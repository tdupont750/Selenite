using System;
using System.Collections.Generic;
using System.Linq;
using Selenite.Global;
using Selenite.Services;
using Xunit.Extensions;

namespace Selenite.Browsers
{
    public class BrowserDataAttribute : DataAttribute
    {
        private static readonly ITestCollectionService TestCollectionService = ServiceResolver.Get<ITestCollectionService>();
        private static readonly IManifestService ManifestService = ServiceResolver.Get<IManifestService>();

        public override IEnumerable<object[]> GetData(System.Reflection.MethodInfo methodUnderTest, Type[] parameterTypes)
        {
            var manifestName = ManifestService.GetActiveManifestName();
            var manifest = ManifestService.GetManifest(manifestName);
            var testCollections = TestCollectionService.GetTestCollections(manifest);

            return testCollections
                .Where(t => t.Enabled)
                .SelectMany(c => c.Tests)
                .Where(t => t.Enabled)
                .Select(t => new object[] { t });
        }
    }
}