using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Selenite.Commands;
using System.Text.RegularExpressions;

namespace Selenite.Models
{
    public class SeleniteTest
    {
        private Regex _macroRegex = new Regex("\\@\\{[a-z0-9A-Z_]+\\}", RegexOptions.Compiled);

        [JsonIgnore]
        public TestCollection TestCollection { get; set; }

        public bool Enabled { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public string Url { get; set; }
        public IList<ICommand> Commands { get; set; }

        [JsonIgnore]
        public string TestUrl { get; set; }

        public IDictionary<string, string> Macros { get; set; }

        public string ResolveMacros(string text)
        {
            if (string.IsNullOrEmpty(text) || (Macros == null && (TestCollection == null || TestCollection.Macros == null)))
                return text;

            return _macroRegex.Replace(text, new MatchEvaluator(ReplaceMacro));
        }

        public string ReplaceMacro(Match match)
        {
            var macroName = match.Value.Substring(2, match.Value.Length - 3);
            string value;

            // try a macro at the test level
            if (Macros == null || !Macros.TryGetValue(macroName, out value))
            {
                // then try it at the collection level
                if (TestCollection.Macros == null || !TestCollection.Macros.TryGetValue(macroName, out value))
                {
                    throw new InvalidOperationException("Could not resolve macro '@{" + macroName + "}'.");
                }
            }

            return ResolveMacros(value);
        }

        public override string ToString()
        {
            return String.Concat(TestCollection.File, " - ", Name);
        }
    }
}