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
                .GetFiles(pathRoot, "*.json")
                .ToList();

            files.Remove(_configurationService.ManifestFileName);
            
            return files;
        }

        public TestCollection GetTestCollection(string testCollectionFile)
        {
            var pathRoot = Path.GetFullPath(_configurationService.TestScriptsPath);
            var path = Path.Combine(pathRoot, testCollectionFile);

            var testCollectionJson = _fileService.ReadAllText(path);
            var testCollection = JObject.Parse(testCollectionJson);

            return CreateTestCollection(testCollectionFile, testCollection);
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
                .Select(GetTestCollection)
                .ToList();

            if (!String.IsNullOrWhiteSpace(manifest.OverrideDomain))
                foreach (var testCollection in testCollections)
                    testCollection.DefaultDomain = manifest.OverrideDomain;

            return testCollections;
        }

        private TestCollection CreateTestCollection(string name, dynamic testCollection)
        {
            var tests = new List<Test>();

            foreach (var test in testCollection.Tests)
                tests.Add(CreateTest(test, testCollection.DefaultDomain.ToString(), testCollection.Name));

            return new TestCollection
            {
                DefaultDomain = testCollection.DefaultDomain,
                Enabled = testCollection.Enabled ?? true,
                File = name,
                Tests = tests
            };
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
                DomainUrl = new Uri(new Uri(domain), url).ToString()
            };
        }
    }
}