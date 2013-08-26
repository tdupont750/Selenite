using System;

// marks an assembly as one that contains Selenite extensions.

namespace Selenite
{
    [AttributeUsage(AttributeTargets.Assembly)]
    public class SeleniteAssemblyAttribute : Attribute
    {
    }
}