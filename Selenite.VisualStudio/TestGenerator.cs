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
        private static readonly Regex TokenFinder = new Regex("\\@\\{[^\\}]+\\}", RegexOptions.Compiled);
        private static readonly Regex DisallowedTestNamePattern = new Regex("[^a-zA-Z0-9_]", RegexOptions.Compiled);

        private const string CodeFileTemplatePrefix = @"using System;
using Xunit;
using Xunit.Extensions;
using Selenite;
using Selenite.Enums;
using Selenite.Models;

@{BeginCompilationSymbol}
namespace @{Namespace}
{";
        private const string CodeFileTemplatePostfix = @"
}
@{EndCompilationSymbol}
";

        private const string TestClassTemplate = @"
    [SeleniteDriver(DriverType.@{DriverType})]
    public class @{ManifestName}_@{DriverType}_Tests : IUseFixture<SeleniteFixture>
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
        [Theory@{SkipParameter}, SeleniteData, SeleniteTestCollection(""@{TestCollectionName}""), SeleniteTest(""@{TestName}""), SeleniteDomainOverride(""@{OverrideDomain}"")]
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
                ? String.Empty
                : GenerateTestClasses(inputFileName, manifest);

            return Encoding.UTF8.GetBytes(code);
        }

        private static bool GetCompilationSymbol(Manifest manifest, out string compilationSymbol)
        {
            const string key = "CompilationSymbol";

            compilationSymbol = String.Empty;

            if (manifest.Metadata == null)
                return false;

            if (!manifest.Metadata.ContainsKey(key))
                return false;

            var value = manifest.Metadata[key];
            return !String.IsNullOrWhiteSpace(value);
        }

        private string GenerateTestClasses(string inputFileName, Manifest manifest)
        {
            var activeProject = Dte.ActiveSolutionProjects[0];
            var projectPath = new Uri((string)activeProject.FileName);
            
            string compilationSymbol;
            var hasCompilationSymbol = GetCompilationSymbol(manifest, out compilationSymbol);
            
            var driverTypes = manifest.DriverTypes.IsNullOrNotAny()
                ? Enum
                    .GetValues(typeof (DriverType))
                    .Cast<DriverType>()
                    .ToArray()
                : manifest.DriverTypes;

            var sb = new StringBuilder();

            var startIf = hasCompilationSymbol ? "#if " + compilationSymbol : String.Empty;
            var firstLine = CodeFileTemplatePrefix.Replace("@{Namespace}", FileNamespace).Replace("@{BeginCompilationSymbol}", startIf);
            sb.AppendLine(firstLine);

            for (var i = 0; i < driverTypes.Count; i++)
            {
                var driverType = driverTypes[i];
                if (driverType == DriverType.Unknown)
                    continue;

                var tokenReplacer = new Func<Match, string>(match =>
                {
                    switch (match.Value)
                    {
                        case "@{ManifestName}":
                            return DisallowedTestNamePattern.Replace(manifest.Name, "_");
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

            var endIf = hasCompilationSymbol ? "#endif" : String.Empty;
            var lastLine = CodeFileTemplatePostfix.Replace("@{EndCompilationSymbol}", endIf); 
            sb.AppendLine(lastLine);

            return sb.ToString();
        }
        
        private string GenerateTestMethods(Uri projectDirectory, string manifestBaseDirectory, Manifest manifest, DriverType driverType)
        {
            var testCollectionService = ServiceResolver.Get<ITestCollectionService>();

            var sb = new StringBuilder();

            foreach (var testCollectionFile in manifest.Files)
            {
                var testCollectionFilePath = Path.Combine(manifestBaseDirectory, testCollectionFile);
                var testCollection = testCollectionService.GetTestCollection(testCollectionFilePath, manifest.OverrideDomain);

                if (!testCollection.DriverTypes.IsNullOrNotAny() && !testCollection.DriverTypes.Contains(driverType))
                    continue;

                var testCollectionFullPath = new Uri(testCollectionFilePath);
                var testCollectionRelativePath = "~/" + projectDirectory.MakeRelativeUri(testCollectionFullPath);

                foreach (var test in testCollection.Tests)
                {
                    if (!test.DriverTypes.IsNullOrNotAny() && !test.DriverTypes.Contains(driverType))
                        continue;

                    sb.AppendLine(GenerateTestMethod(testCollectionRelativePath, manifest, test));
                }

            }

            return sb.ToString();
        }

        private string GenerateTestMethod(string testCollectionRelativePath, Manifest manifest, SeleniteTest test)
        {
            var tokenReplacer = new Func<Match, string>(match =>
            {
                switch (match.Value)
                {
                    case "@{TestName}":
                        return test.Name;
                    case "@{OverrideDomain}":
                        return manifest.OverrideDomain;
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
