using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Selenite.Commands;
using Selenite.Models;

namespace Selenite.Services.Implementation
{
    public class TestCollectionService : ITestCollectionService
    {
        private readonly IConfigurationService _configurationService;
        private readonly IFileService _fileService;
        private readonly ICommandService _commandService;

        public TestCollectionService(IConfigurationService configurationService, IFileService fileService, ICommandService commandService)
        {
            _configurationService = configurationService;
            _fileService = fileService;
            _commandService = commandService;
        }

        public IList<string> GetTestCollectionFiles()
        {
            var pathRoot = Path.GetFullPath(_configurationService.TestScriptsPath);

            var files = _fileService
                .GetFiles(pathRoot, "*.json") // CCHINN: This shouldn't just look at the path of the manifest file, it should find all the files referenced by manifests.
                .ToList();

            var manifestFile = files.FirstOrDefault(f => f.EndsWith(_configurationService.ManifestFileName, StringComparison.InvariantCultureIgnoreCase));
            files.Remove(manifestFile);
            
            return files;
        }

        public TestCollection GetTestCollection(string testCollectionFile, string overrideDomain = null)
        {
            var pathRoot = Path.GetFullPath(_configurationService.TestScriptsPath);
            var path = Path.Combine(pathRoot, testCollectionFile);

            var testCollectionJson = _fileService.ReadAllText(path);
            var testCollection = JObject.Parse(testCollectionJson);

            return CreateTestCollection(testCollectionFile, testCollection, overrideDomain);
        }

        public void SaveTestCollection(TestCollection testCollection)
        {
            var pathRoot = Path.GetFullPath(_configurationService.TestScriptsPath);
            var path = Path.Combine(pathRoot, testCollection.File);

            var testCollectionJson = JsonConvert.SerializeObject(testCollection, Formatting.Indented);
            _fileService.WriteAllText(path, testCollectionJson);
        }

        public IList<TestCollection> GetTestCollections(Manifest manifest)
        {
            var testCollections = manifest.Files
                .Select(file => GetTestCollection(file, manifest.OverrideDomain))
                .ToList();

            return testCollections;
        }

        private TestCollection CreateTestCollection(string name, dynamic testCollection, string overrideDomain)
        {
            var collection = new TestCollection
                {
                    DefaultDomain = string.IsNullOrWhiteSpace(overrideDomain)
                        ? testCollection.DefaultDomain
                        : overrideDomain,
                    Enabled = testCollection.Enabled ?? true,
                    File = name,
                };

            var tests = new List<Test>();

            foreach (var test in testCollection.Tests)
                tests.Add(CreateTest(test, collection.DefaultDomain, name));

            collection.Tests = tests;

            return collection;
        }

        private Test CreateTest(dynamic test, string domain, string testCollectionName)
        {
            var commands = new List<ICommand>();

            foreach (var command in test.Commands)
                commands.Add(_commandService.CreateCommand(command));

            var url = test.Url.ToString();

            return new Test
            {
                CollectionName = testCollectionName,
                Commands = commands,
                Enabled = test.Enabled ?? true,
                Name = test.Name,
                Url = url,
                TestUrl = new Uri(new Uri(domain + "/"), url).ToString() // Add a slash to the end of the URL in case it isn't there; URI's ignore multiple slashies.
            };
        }
    }
}