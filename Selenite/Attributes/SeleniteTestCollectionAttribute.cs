using System;
using System.Collections.Generic;

namespace Selenite
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class SeleniteTestCollectionAttribute : Attribute
    {
        public IList<string> Collections { get; private set; }

        public SeleniteTestCollectionAttribute(params string[] collections)
        {
            Collections = collections ?? new string[0];
        }
    }
}