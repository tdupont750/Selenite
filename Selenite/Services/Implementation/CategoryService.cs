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
    public class CategoryService : ICategoryService
    {
        public const string TestExtension = "test";

        private readonly IConfigurationService _configurationService;
        private readonly IFileService _fileService;
        private readonly ICommandService _commandService;

        public CategoryService(IConfigurationService configurationService, IFileService fileService, ICommandService commandService)
        {
            _configurationService = configurationService;
            _fileService = fileService;
            _commandService = commandService;
        }

        public IList<string> GetCategoryNames()
        {
            var pathRoot = Path.GetFullPath(_configurationService.TestsPath);

            return _fileService
                .GetFiles(pathRoot, "*." + TestExtension)
                .Select(Path.GetFileNameWithoutExtension)
                .ToList();
        }

        public Category GetCategory(string categoryName)
        {
            var pathRoot = Path.GetFullPath(_configurationService.TestsPath);
            var path = Path.Combine(pathRoot, categoryName + "." + TestExtension);

            var categoryJson = _fileService.ReadAllText(path);
            var category = JObject.Parse(categoryJson);

            return CreateCategory(categoryName, category);
        }

        public void SaveCategory(Category category)
        {
            var pathRoot = Path.GetFullPath(_configurationService.TestsPath);
            var path = Path.Combine(pathRoot, category.Name + "." + TestExtension);

            var categoryJson = JsonConvert.SerializeObject(category, Formatting.Indented);
            _fileService.WriteAllText(path, categoryJson);
        }

        public IList<Category> GetCategories(Manifest manifest)
        {
            var categories = manifest.Categories
                .Select(GetCategory)
                .ToList();

            if (!String.IsNullOrWhiteSpace(manifest.Domain))
                foreach (var category in categories)
                    category.Domain = manifest.Domain;

            return categories;
        }

        private Category CreateCategory(string name, dynamic category)
        {
            var tests = new List<Test>();

            foreach (var test in category.Tests)
                tests.Add(CreateTest(test, category.Domain.ToString(), category.Name));

            return new Category
            {
                Domain = category.Domain,
                Enabled = category.Enabled ?? true,
                Name = name,
                Tests = tests
            };
        }

        private Test CreateTest(dynamic test, string domain, string categoryName)
        {
            var commands = new List<ICommand>();

            foreach (var command in test.Commands)
                commands.Add(_commandService.CreateCommand(command));

            var url = test.Url.ToString();

            return new Test
            {
                CategoryName = categoryName,
                Commands = commands,
                Enabled = test.Enabled ?? true,
                Name = test.Name,
                Url = url,
                DomainUrl = new Uri(new Uri(domain), url).ToString()
            };
        }
    }
}