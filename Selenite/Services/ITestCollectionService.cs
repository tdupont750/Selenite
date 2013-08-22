using System.Collections.Generic;
using Selenite.Models;

namespace Selenite.Services
{
    public interface ITestCollectionService
    {
        IList<string> GetTestCollectionFiles();
        TestCollection GetTestCollection(string testCollectionFile, string domainOverride = null);
        void SaveTestCollectionInfo(TestCollection testCollection);
        IList<TestCollection> GetTestCollections(Manifest manifest, string overrideDomain = null);
        IList<TestCollection> GetTestCollections(IList<string> testCollectionFiles, string overrideDomain = null);
    }
}