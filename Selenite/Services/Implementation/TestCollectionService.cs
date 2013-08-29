using System.Net.Mime;
using Newtonsoft.Json.Linq;
using Selenite.Commands;
using Selenite.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Selenite.Services.Implementation
{
#pragma warning disable 1591
    public class TestCollectionService : ITestCollectionService
#pragma warning restore 1591
    {
        private readonly IConfigurationService _configurationService;
        private readonly IFileService _fileService;
        private readonly ICommandService _commandService;
        private readonly IManifestService _manifestService;

        public TestCollectionService(IConfigurationService configurationService, IFileService fileService, ICommandService commandService, IManifestService manifestService)
        {
            _configurationService = configurationService;
            _fileService = fileService;
            _commandService = commandService;
            _manifestService = manifestService;
        }

        public IList<string> GetTestCollectionFiles()
        {
            var activeManifest = _manifestService.GetManifest(_manifestService.GetActiveManifestName());

            return activeManifest.Files;
        }

        private string _currentApplicationBaseDirectory;
        private string CurrentApplicationBaseDirectory {
            get
            {
                if (_currentApplicationBaseDirectory == null)
                {
                    // todo: GET RID OF THIS CRAP

                    var dir = AppDomain.CurrentDomain.BaseDirectory;
                    while (!File.Exists(Path.Combine(dir, "app.config")) && !File.Exists(Path.Combine(dir, "web.config")))
                    {
                        dir = Path.Combine(dir, "..");
                        if (string.IsNullOrEmpty(dir))
                        {
                            dir = null;
                            break;
                        }
                    }

                    _currentApplicationBaseDirectory = dir;
                }
                return _currentApplicationBaseDirectory;
            }
        }

        public TestCollection GetTestCollection(string testCollectionFile, string overrideDomain = null)
        {
            var path = testCollectionFile;
            if (!Path.IsPathRooted(testCollectionFile))
            {
                if (path.StartsWith("~"))
                {
                    if (CurrentApplicationBaseDirectory == null)
                        throw new InvalidOperationException("Could not determine the project root directory.");

                    path = path.Replace("~", CurrentApplicationBaseDirectory);
                }
                else
                {
                    var pathRoot = Path.GetFullPath(_configurationService.TestScriptsPath);
                    path = Path.Combine(pathRoot, testCollectionFile);
                }
            }

            var testCollectionJson = _fileService.ReadAllText(path);
            var testCollection = JObject.Parse(testCollectionJson);

            return CreateTestCollection(testCollectionFile, testCollection, overrideDomain);
        }

        public void SaveTestCollectionInfo(TestCollection testCollection)
        {
            var activeManifest = _configurationService.ActiveManifestInfo;

            if (activeManifest == null)
            {
                throw new InvalidOperationException("Could not load the active manifest.");
            }

            var testCollectionInfo = new TestCollectionInfo
            {
                Name = testCollection.File,
                IsEnabled = testCollection.Enabled,
                DisabledTests = testCollection.Tests
                    .Where(test => !test.Enabled)
                    .Select(test => test.Name)
                    .ToList(),
            };

            if (activeManifest.TestCollections == null)
            {
                activeManifest.TestCollections = new List<TestCollectionInfo> { testCollectionInfo };
            }

            // Update the existing test collection to the current values if it's already in the list.
            var tci = activeManifest.TestCollections.FirstOrDefault(tc => tc.Name == testCollectionInfo.Name);
            if (tci != null)
            {
                activeManifest.TestCollections.Remove(tci);
            }

            activeManifest.TestCollections.Add(testCollectionInfo);

            _configurationService.ActiveManifestInfo = activeManifest;
        }

        public IList<TestCollection> GetTestCollections(Manifest manifest, string overrideDomain = null)
        {
            var testCollections = manifest.Files
                .Select(file => GetTestCollection(file, overrideDomain ?? manifest.OverrideDomain))
                .ToList();

            var manifestInfo = _configurationService.ActiveManifestInfo;

            // Go through the manifest info and see if we need to disable any tests.
            foreach (var testCollection in testCollections)
            {
                var tc = manifestInfo.TestCollections != null
                    ? manifestInfo.TestCollections.FirstOrDefault(testColl => testColl.Name == testCollection.File)
                    : null;

                if (tc == null)
                    continue;

                testCollection.Enabled = tc.IsEnabled;

                foreach (var test in testCollection.Tests)
                    if (tc.DisabledTests.Contains(test.Name))
                        test.Enabled = false;
            }

            return testCollections;
        }

        public IList<TestCollection> GetTestCollections(IList<string> testCollectionFiles, string overrideDomain)
        {
            return testCollectionFiles
                .Select(file => GetTestCollection(file, overrideDomain))
                .ToList();
        }

        // TODO: See if the UI can go through the same flow as the test runner and use GetTestCollections instead of this method.
        private TestCollection CreateTestCollection(string name, dynamic testCollection, string overrideDomain)
        {
            var manifestInfo = _configurationService == null
                ? null
                : _configurationService.ActiveManifestInfo;

            var testCollectionInfo = manifestInfo != null && manifestInfo.TestCollections != null
                ? manifestInfo.TestCollections.FirstOrDefault(tc => tc.Name == name)
                : null;

            var isEnabled = testCollectionInfo != null
                ? testCollectionInfo.IsEnabled
                : testCollection.Enabled ?? true;

            var collection = new TestCollection
            {
                DefaultDomain = string.IsNullOrWhiteSpace(overrideDomain)
                    ? testCollection.DefaultDomain
                    : overrideDomain,
                Enabled = isEnabled,
                File = name,
                Description = testCollection.Description,
                Macros = GetDictionaryFromJObject(testCollection.Macros),
            };

            var tests = new List<SeleniteTest>();

            foreach (var test in testCollection.Tests)
            {
                var enabled = (bool?) test.Enabled;

                var testEnabled = (testCollectionInfo == null 
                        || testCollectionInfo.DisabledTests == null 
                        || !testCollectionInfo.DisabledTests.Any(testName => testName == test.Name.ToString()))
                    && enabled.GetValueOrDefault(true);

                tests.Add(CreateTest(collection, test, collection.DefaultDomain, testEnabled));
            }

            if (testCollection.SetupStepsFile != null && !String.IsNullOrWhiteSpace(testCollection.SetupStepsFile.ToString()))
            {
                if (testCollection.SetupSteps != null)
                    throw new InvalidOperationException("Must only specify SetupStepsFile or SetupStepsFile");

                var setupStepsJson = _fileService.ReadAllText(testCollection.SetupStepsFile.ToString());
                testCollection.SetupSteps = JArray.Parse(setupStepsJson);
            }
            
            if (testCollection.SetupSteps != null)
            {
                var setupSteps = new List<SeleniteTest>();
                foreach (var step in testCollection.SetupSteps)
                {
                    var setupStep = CreateTest(collection, step, collection.DefaultDomain, true);
                    setupSteps.Add(setupStep);
                }
                collection.SetupSteps = setupSteps;
            }

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

        private SeleniteTest CreateTest(TestCollection testCollection, dynamic test, string domain, bool isEnabled = true)
        {
            var url = test.Url.ToString();

            var baseUri = domain.EndsWith("/")
                ? new Uri(domain)
                : new Uri(domain + "/");

            var relativeUri = new Uri(baseUri, url);

            var testInstance = new SeleniteTest
            {
                TestCollection = testCollection,
                Enabled = isEnabled,
                Name = test.Name,
                Description = test.Description,
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