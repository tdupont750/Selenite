using System;
using System.Linq;
using Moq;
using Selenite.Commands.Implementation;
using Selenite.Services;
using Selenite.Services.Implementation;
using Xunit;
using Xunit.Extensions;

namespace Selenite.Tests.Services
{
    public class TestCollectionServiceTests
    {
        public const string Test = @"{
  ""Enabled"": true,
  ""DefaultDomain"": ""http://google.com/"",
  ""Tests"": [
    {
      ""Enabled"": true,
      ""Name"": ""Basic"",
      ""Url"": """",
      ""Commands"": [
        {
          ""Title"": ""Google"",
          ""IsCaseSensitive"": false,
          ""IsFalseExpected"": false,
          ""Name"": ""IsTitleEqual""
        }
      ]
    }
  ]
}";

        public string WriteAllTextValue { get; private set; }

        private ITestCollectionService GetTestCollectionService(string name, string json)
        {
            var configurationService = new Mock<IConfigurationService>();
            configurationService
                .SetupGet(p => p.TestScriptsPath)
                .Returns("C:\\");

            var path = String.Format("C:\\{0}", name);

            var fileService = new Mock<IFileService>();
            fileService
                .Setup(f => f.ReadAllText(path))
                .Returns(json);
            fileService
                .Setup(f => f.WriteAllText(path, It.IsAny<string>()))
                .Callback<string, string>((p, c) => WriteAllTextValue = c);

            var commandService = new CommandService();

            return new TestCollectionService(configurationService.Object, fileService.Object, commandService);
        }

        [Theory]
        [InlineData("Test.json", Test)]
        public void GetTestCollection(string name, string json)
        {
            var testCollectionService = GetTestCollectionService(name, json);

            var testCollection = testCollectionService.GetTestCollection(name);
            Assert.Equal(1, testCollection.Tests.Count);

            var test = testCollection.Tests.First();
            Assert.Equal(1, test.Commands.Count);
            Assert.Equal(String.Empty, test.Url);
            Assert.Equal("http://google.com/", test.TestUrl);

            var command = (IsTitleEqualCommand) test.Commands.First();
            Assert.Equal("IsTitleEqual", command.Name);
            Assert.Equal("Google", command.Title);
            Assert.Equal(false, command.IsCaseSensitive);
        }

        [Theory]
        [InlineData("Test.json", Test)]
        public void SaveTestCollection(string name, string json)
        {
            var testCollectionService = GetTestCollectionService(name, json);

            var testCollection = testCollectionService.GetTestCollection(name);
            testCollectionService.SaveTestCollection(testCollection);

            Assert.Equal(json, WriteAllTextValue);
        }
    }
}
