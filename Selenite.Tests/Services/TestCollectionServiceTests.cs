using System;
using System.IO;
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
        private ITestCollectionService GetTestCollectionService(params string[] filesAndValues)
        {
            var configurationService = new Mock<IConfigurationService>();
            configurationService
                .SetupGet(p => p.TestScriptsPath)
                .Returns("C:\\");

            var fileService = new Mock<IFileService>();

            for (var i = 0; i < filesAndValues.Length; i += 2)
            {
                var path = filesAndValues[i].StartsWith("file:///")
                    ? filesAndValues[i]
                    : String.Concat("C:\\", filesAndValues[i]);

                fileService
                    .Setup(f => f.ReadAllText(path))
                    .Returns(filesAndValues[i + 1]);
            }

            var commandService = new CommandService();
            var manifestService = new Mock<IManifestService>();

            return new TestCollectionService(configurationService.Object, fileService.Object, commandService, manifestService.Object);
        }

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

        [Theory]
        [InlineData("Test.json", Test)]
        public void GetTestCollection(string name, string path)
        {
            var testCollectionService = GetTestCollectionService(name, path);

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

        public const string Test2 = @"{
  ""Enabled"": true,
  ""DefaultDomain"": ""http://google.com/"",
  ""SetupSteps"": [
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
  ],
  ""Tests"": []
}";

        [Theory]
        [InlineData("Test2.json", Test2)]
        public void SetupStepsEmbedded(string name, string path)
        {
            var testCollectionService = GetTestCollectionService(name, path);

            var testCollection = testCollectionService.GetTestCollection(name);
            Assert.Equal(0, testCollection.Tests.Count);
            Assert.Equal(1, testCollection.SetupSteps.Count);

            var setupStep = testCollection.SetupSteps.First();
            Assert.Equal(1, setupStep.Commands.Count);
            Assert.Equal(String.Empty, setupStep.Url);
            Assert.Equal("http://google.com/", setupStep.TestUrl);

            var command = (IsTitleEqualCommand)setupStep.Commands.First();
            Assert.Equal("IsTitleEqual", command.Name);
            Assert.Equal("Google", command.Title);
            Assert.Equal(false, command.IsCaseSensitive);
        }

        public const string Test3A = @"{
  ""Enabled"": true,
  ""DefaultDomain"": ""http://google.com/"",
  ""SetupStepsFile"": ""C:\\Test3B.json"",
  ""Tests"": []
}";

        public const string Test3B = @"[
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
]";

        [Theory]
        [InlineData("Test3A.json", Test3A, "file:///C:/Test3B.json", Test3B)]
        public void SetupStepsFile(string nameA, string pathA, string nameB, string pathB)
        {
            var testCollectionService = GetTestCollectionService(nameA, pathA, nameB, pathB);

            var testCollection = testCollectionService.GetTestCollection(nameA);
            Assert.Equal(0, testCollection.Tests.Count);
            Assert.Equal(1, testCollection.SetupSteps.Count);

            var setupStep = testCollection.SetupSteps.First();
            Assert.Equal(1, setupStep.Commands.Count);
            Assert.Equal(String.Empty, setupStep.Url);
            Assert.Equal("http://google.com/", setupStep.TestUrl);

            var command = (IsTitleEqualCommand)setupStep.Commands.First();
            Assert.Equal("IsTitleEqual", command.Name);
            Assert.Equal("Google", command.Title);
            Assert.Equal(false, command.IsCaseSensitive);
        }

        public const string Test4 = @"{
  ""Enabled"": true,
  ""DefaultDomain"": ""http://google.com/"",
  ""SetupStepsFile"": ""C:\\Test3B.json"",
  ""SetupSteps"": [
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
  ],
  ""Tests"": []
}";

        [Theory]
        [InlineData("Test4.json", Test4)]
        public void SetupStepsFail(string name, string path)
        {
            var testCollectionService = GetTestCollectionService(name, path);

            var exception = Assert.Throws<InvalidOperationException>(() =>
            {
                testCollectionService.GetTestCollection(name);
            });

            Assert.Equal("Must only specify SetupSteps or SetupStepsFile", exception.Message);
        }

        [Fact]
        public void Meh()
        {
            var x = Path.Combine("c:/hello/", "../world/meh.text");
            var uri = new Uri(x);

            var y = Path.Combine("c:/hello/", "d:/world/meh.text");
            var uri2 = new Uri(y);
        }
    }
}
