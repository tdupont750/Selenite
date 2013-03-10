using System;
using Moq;
using Selenite.Services;
using Selenite.Services.Implementation;
using Xunit;
using Xunit.Extensions;

namespace Selenite.Tests.Services
{
    public class ManifestServiceTests
    {
        public const string Test = @"{
	ActiveManifest: ""Second"",
	Manifests: [{
		Name: ""First"",
		DomainOverride: """",
		Files: [""A.json"", ""B.json""]
	}, {
		Name: ""Second"",
		DomainOverride: ""http://www.google.com/"",
		Files: [""B.json"", ""C.json""]
	}, {
		Name: ""Third"",
		DomainOverride: ""http://www.bing.com/"",
		Files: [""C.json"", ""D.json""]
	}]
}";

        public string WriteAllTextValue { get; private set; }

        private IManifestService GetManifestService(string name, string json)
        {
            var configurationService = new Mock<IConfigurationService>();
            configurationService
                .SetupGet(p => p.TestScriptsPath)
                .Returns("C:\\");
            configurationService
                .SetupGet(p => p.ManifestFileName)
                .Returns(name);

            var path = String.Format("C:\\{0}", name);

            var fileService = new Mock<IFileService>();
            fileService
                .Setup(f => f.ReadAllText(path))
                .Returns(json);
            fileService
                .Setup(f => f.WriteAllText(path, It.IsAny<string>()))
                .Callback<string, string>((p, c) => WriteAllTextValue = c);

            return new ManifestService(configurationService.Object, fileService.Object);
        }

        [Theory]
        [InlineData(".manifests.json", Test)]
        public void GetManifestNames(string name, string json)
        {
            var manifesetService = GetManifestService(name, json);
            var manifestNames = manifesetService.GetManifestNames();

            Assert.Equal(3, manifestNames.Count);
            Assert.Equal("First", manifestNames[0]);
            Assert.Equal("Second", manifestNames[1]);
            Assert.Equal("Third", manifestNames[2]);
        }

        [Theory]
        [InlineData(".manifests.json", Test)]
        public void GetActiveManifestName(string name, string json)
        {
            var manifesetService = GetManifestService(name, json);
            var activeManifestName = manifesetService.GetActiveManifestName();

            Assert.Equal("Second", activeManifestName);

            var activeManifest = manifesetService.GetManifest(activeManifestName);

            Assert.Equal("Second", activeManifest.Name);
            Assert.Equal(2, activeManifest.Files.Count);
            Assert.Equal("B.json", activeManifest.Files[0]);
            Assert.Equal("C.json", activeManifest.Files[1]);
        }
    }
}