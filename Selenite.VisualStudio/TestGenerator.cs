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

namespace Selenite.VisualStudio
{
    [ComVisible(true)]
    [Guid("9CF37C98-B662-4B96-9B01-C3D5EA24FC00")]
    public class TestGenerator : BaseCodeGeneratorWithSite
    {
        private static Regex TokenFinder = new Regex("\\@\\{.+\\}", RegexOptions.Compiled);
        private static Regex DisallowedTestNamePattern = new Regex("[^a-zA-Z0-9_]", RegexOptions.Compiled);

        private const string TestClassTemplate = @"
using System;
using Xunit;
using Xunit.Extensions;

namespace Selenite.Tests
{

    [SeleniteTestCollection(""@{InputFileName}"")]
    public class @{FileName}Tests
    {
        
@{TestMethods}

    }
}
";
        private const string TestMethodTemplate = @"
        [SeleniteTest, Fact, Trait(""Selenite"", ""True"")]
        public void @{TestName}()
        {
            /*
@{TestJson}
            */
        }
";

        public override string GetDefaultExtension()
        {
            return ".cs";
        }

        protected override byte[] GenerateCode(string inputFileName, string inputFileContent)
        {
            var testCollection = (dynamic) JObject.Parse(inputFileContent);
            var code = GenerateTestClass(inputFileName, testCollection);

            return Encoding.UTF8.GetBytes(code);
        }

        private string GenerateTestClass(string inputFileName, dynamic testCollection)
        {
            var tokenReplacer = new Func<Match, string>(match =>
            {
                switch (match.Value)
                {
                    case "@{FileName}":
                        return Path.GetFileNameWithoutExtension(inputFileName);
                    case "@{InputFileName}":
                        return Path.GetFileName(inputFileName);
                    case "@{TestMethods}":
                        return GenerateTestMethods(testCollection);
                    default:
                        throw new InvalidOperationException("Invalid token: " + match.Value);
                }
            });

            var code = TestClassTemplate;
            code = TokenFinder.Replace(code, new MatchEvaluator(tokenReplacer));
            return code;
        }

        private string GenerateTestMethods(dynamic testCollection)
        {
            var sb = new StringBuilder();
            foreach (var test in testCollection.Tests)
            {
                sb.AppendLine(GenerateTestMethod(test));
            }

            return sb.ToString();
        }

        private string GenerateTestMethod(dynamic test)
        {
            var tokenReplacer = new Func<Match, string>(match =>
            {
                switch (match.Value)
                {
                    case "@{TestName}":
                        return DisallowedTestNamePattern.Replace((string)test.Name, string.Empty);
                    case "@{TestJson}":
                        return JsonConvert.SerializeObject(test, Formatting.Indented);
                    default:
                        throw new InvalidOperationException("Invalid token: " + match.Value);
                }
            });
            var code = TestMethodTemplate;
            return TokenFinder.Replace(code, new MatchEvaluator(tokenReplacer));
        }
    }
}
