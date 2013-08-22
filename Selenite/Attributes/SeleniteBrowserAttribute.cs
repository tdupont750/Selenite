using System;
using System.Collections.Generic;
using Selenite.Enums;

namespace Selenite
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class SeleniteDriverAttribute : Attribute
    {
        public IList<DriverType> DriverTypes { get; private set; }

        public SeleniteDriverAttribute(params DriverType[] driverTypes)
        {
            DriverTypes = driverTypes ?? new DriverType[0];
        }
    }
}