using System.Collections.Generic;
using Selenite.Models;

namespace Selenite.Services
{
    public interface ITestCollectionService
    {
        IList<string> GetTestCollectionFiles();
        TestCollection GetTestCollection(string testCollectionFile, string domainOverride = null);
        void SaveTestCollection(TestCollection testCollection);
        IList<TestCollection> GetTestCollections(Manifest manifest);
    }
}