using System.Collections.Generic;
using Selenite.Models;

namespace Selenite.Services
{
    public interface ITestCollectionService
    {
        IList<string> GetTestCollectionFiles();
        TestCollection GetTestCollection(string testCollectionFile);
        void SaveTestCollection(TestCollection testCollection);
        IList<TestCollection> GetTestCollections(Manifest manifest);
    }
}