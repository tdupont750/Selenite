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

            // TODO: This shouldn't just look at the path of the manifest file, it should find all the files referenced by manifests.
            var files = _fileService
                .GetFiles(pathRoot, "*.json")
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

            var testCollectionJson = JsonConvert.SerializeObject(testCollection, Formatting.Indented, new JsonSerializerSettings { DefaultValueHandling = DefaultValueHandling.Ignore });
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
                    Macros = GetDictionaryFromJObject(testCollection.Macros)
                };

            var tests = new List<Test>();

            foreach (var test in testCollection.Tests)
                tests.Add(CreateTest(collection, test, collection.DefaultDomain, name));

            collection.Tests = tests;

            return collection;
        }

        private IDictionary<string, string> GetDictionaryFromJObject(JObject obj)
        {
            IDictionary<string, string> dictionary = null;

            IDictionary<string, JToken> jobjectMacros = obj;
            if (jobjectMacros != null)
            {
                dictionary = new Dictionary<string, string>();
                foreach (var b in jobjectMacros)
                {
                    dictionary[b.Key] = b.Value.ToString();
                }
            }
            return dictionary;
        }

        private Test CreateTest(TestCollection testCollection, dynamic test, string domain, string testCollectionName)
        {
            var url = test.Url.ToString();

            var baseUri = domain.EndsWith("/")
                ? new Uri(domain)
                : new Uri(domain + "/");

            var relativeUri = new Uri(baseUri, url);

            var testInstance = new Test
            {
                CollectionName = testCollectionName,
                TestCollection = testCollection,
                Enabled = test.Enabled ?? true,
                Name = test.Name,
                Url = url,
                TestUrl = relativeUri.ToString(),
                Macros = GetDictionaryFromJObject(test.Macros)
            };

            var commands = new List<ICommand>();

            foreach (var command in test.Commands)
                commands.Add(_commandService.CreateCommand(command, testInstance));

            testInstance.Commands = commands;

            return testInstance;
        }
    }
}