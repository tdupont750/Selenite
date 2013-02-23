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
    public class CategoryServiceTests
    {
        public const string Test = @"{
  ""Enabled"": true,
  ""Domain"": ""http://google.com/"",
  ""Tests"": [
    {
      ""CategoryName"": null,
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

        public string WriteAllTextValue { get; set; }

        public ICategoryService GetCategoryService(string name, string json)
        {
            var configurationService = new Mock<IConfigurationService>();
            configurationService
                .SetupGet(p => p.TestsPath)
                .Returns("C:\\");

            var path = String.Format("C:\\{0}.{1}", name, CategoryService.TestExtension);

            var fileService = new Mock<IFileService>();
            fileService
                .Setup(f => f.ReadAllText(path))
                .Returns(json);
            fileService
                .Setup(f => f.WriteAllText(path, It.IsAny<string>()))
                .Callback<string, string>((p, c) => WriteAllTextValue = c);

            var commandService = new CommandService();

            return new CategoryService(configurationService.Object, fileService.Object, commandService);
        }

        [Theory]
        [InlineData("Test", Test)]
        public void GetCategory(string name, string json)
        {
            var categoryService = GetCategoryService(name, json);
            
            var testCategory = categoryService.GetCategory(name);
            Assert.Equal(1, testCategory.Tests.Count);

            var test = testCategory.Tests.First();
            Assert.Equal(1, test.Commands.Count);
            Assert.Equal(String.Empty, test.Url);
            Assert.Equal("http://google.com/", test.DomainUrl);

            var command = (IsTitleEqualCommand) test.Commands.First();
            Assert.Equal("IsTitleEqual", command.Name);
            Assert.Equal("Google", command.Title);
            Assert.Equal(false, command.IsCaseSensitive);
        }

        [Theory]
        [InlineData("Test", Test)]
        public void SaveCategory(string name, string json)
        {
            var categoryService = GetCategoryService(name, json);

            var testCategory = categoryService.GetCategory(name);
            categoryService.SaveCategory(testCategory);

            Assert.Equal(json, WriteAllTextValue);
        }
    }
}
