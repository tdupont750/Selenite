using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TextTemplating.VSHost;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Selenite.Enums;
using Selenite.Global;
using Selenite.Models;
using Selenite.Services;
using Selenite.Services.Implementation;

namespace Selenite.VisualStudio
{
    [ComVisible(true)]
    [Guid("9CF37C98-B662-4B96-9B01-C3D5EA24FC00")]
    public class TestGenerator : BaseCodeGeneratorWithSite
    {
        private static Regex TokenFinder = new Regex("\\@\\{[^\\}]+\\}", RegexOptions.Compiled);
        private static Regex DisallowedTestNamePattern = new Regex("[^a-zA-Z0-9_]", RegexOptions.Compiled);

        private const string CodeFileTemplatePrefix = @"using System;
using Xunit;
using Xunit.Extensions;
using Selenite;
using Selenite.Enums;
using Selenite.Models;

namespace @{Namespace}
{";
        private const string CodeFileTemplatePostfix = @"
}
";

        private const string TestClassTemplate = @"
    [SeleniteDriver(DriverType.@{DriverType})]
    public class @{DriverType}_Tests : IUseFixture<SeleniteFixture>
    {
        public SeleniteFixture SeleniteFixture { get; private set; }

        public void SetFixture(SeleniteFixture data)
        {
            SeleniteFixture = data;
        }

@{TestMethods}

    }
";
        private const string TestMethodTemplate = @"
        [Theory@{SkipParameter}, SeleniteData, SeleniteTestCollection(""@{TestCollectionName}""), SeleniteTest(""@{TestName}"")]
        public void @{TestCollectionNameFriendly}_@{TestNameFriendly}(DriverType driverType, SeleniteTest test)
        {/*
@{TestJson}
         */
            SeleniteFixture.ExecuteTest(driverType, test);
        }
";

        public override string GetDefaultExtension()
        {
            return ".cs";
        }

        protected override byte[] GenerateCode(string inputFileName, string inputFileContent)
        {
            var manifestCollection = JsonConvert.DeserializeObject<ManifestCollection>(inputFileContent);
            var manifest = manifestCollection.Manifests.FirstOrDefault(m => m.Name == manifestCollection.ActiveManifest);
            var code = manifest == null
                ? string.Empty
                : GenerateTestClasses(inputFileName, manifest);

            return Encoding.UTF8.GetBytes(code);
        }

        private string GenerateTestClasses(string inputFileName, Manifest manifest)
        {
            var activeProject = Dte.ActiveSolutionProjects[0];
            var projectPath = new Uri((string)activeProject.FileName);

            var sb = new StringBuilder();
            sb.AppendLine(CodeFileTemplatePrefix.Replace("@{Namespace}", FileNamespace));

            // todo: should we be able to limit which drivers we generate code for? how? config? something in the manifest?
            foreach (var driverType in Enum.GetValues(typeof (DriverType)).Cast<DriverType>())
            {
                if (driverType == DriverType.Unknown)
                    continue;

                
                var tokenReplacer = new Func<Match, string>(match =>
                {
                    switch (match.Value)
                    {
                        case "@{TestMethods}":
                            return GenerateTestMethods(projectPath, Path.GetDirectoryName(inputFileName), manifest, driverType);
                        case "@{DriverType}":
                            return driverType.ToString();
                        default:
                            throw new InvalidOperationException("Invalid token: " + match.Value);
                    }
                });

                var code = TokenFinder.Replace(TestClassTemplate, new MatchEvaluator(tokenReplacer));
                sb.AppendLine(code);
            }

            sb.AppendLine(CodeFileTemplatePostfix);
            return sb.ToString();
        }

        private string GenerateTestMethods(Uri projectDirectory, string manifestBaseDirectory, Manifest manifest, DriverType driverType)
        {
            var testCollectionSerivce = ServiceResolver.Get<ITestCollectionService>();

            var sb = new StringBuilder();
            foreach (var testCollectionFile in manifest.Files)
            {
                var testCollection = testCollectionService.GetTestCollection(
                    Path.Combine(manifestBaseDirectory, testCollectionFile),
                    manifest.OverrideDomain);

                var testCollectionFullPath = new Uri(Path.Combine(manifestBaseDirectory, testCollectionFile));
                var testCollectionRelativePath = "~/" + projectDirectory.MakeRelativeUri(testCollectionFullPath);

                foreach (var test in testCollection.Tests)
                {
                    sb.AppendLine(GenerateTestMethod(testCollectionRelativePath, test));
                }

            }
            return sb.ToString();

        }

        private string GenerateTestMethod(string testCollectionRelativePath, SeleniteTest test)
        {
            var tokenReplacer = new Func<Match, string>(match =>
            {
                switch (match.Value)
                {
                    case "@{TestName}":
                        return test.Name;
                    case "@{TestCollectionName}":
                        return testCollectionRelativePath;
                    case "@{TestCollectionNameFriendly}":
                        var extension = Path.GetExtension(testCollectionRelativePath);
                        var pathWithoutExtension = testCollectionRelativePath.Substring(0, testCollectionRelativePath.Length - extension.Length);
                        var cleanName = DisallowedTestNamePattern.Replace(pathWithoutExtension, "_");
                        cleanName = cleanName.TrimStart('_');
                        return cleanName;
                    case "@{TestNameFriendly}":
                        return DisallowedTestNamePattern.Replace(test.Name, "_");
                    case "@{TestJson}":
                        return JsonConvert.SerializeObject(test, Formatting.Indented);
                    case "@{SkipParameter}":
                        if (!test.TestCollection.Enabled)
                            return "(Skip = \"Disabled in Test Collection JSON\")";
                        if (!test.Enabled)
                            return "(Skip = \"Disabled in Test JSON\")";
                        return string.Empty;
                    default:
                        throw new InvalidOperationException("Invalid token: " + match.Value);
                }
            });

            return TokenFinder.Replace(TestMethodTemplate, new MatchEvaluator(tokenReplacer));
        }
    }
}
