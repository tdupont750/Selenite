using System;
using System.Collections.Generic;

namespace Selenite
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class SeleniteTestAttribute : Attribute
    {
        public IList<string> Tests { get; private set; }

        public SeleniteTestAttribute(params string[] tests)
        {
            Tests = tests ?? new string[0];
        }
    }
}