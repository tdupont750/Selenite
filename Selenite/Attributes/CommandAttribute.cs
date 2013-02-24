using System;
using Selenite.Enums;

namespace Selenite.Attributes
{
    public class CommandAttribute : Attribute
    {
    }

    public class DriverTypeAttribute : Attribute
    {
        DriverTypeAttribute(DriverType driverType)
        {
            DriverType = driverType;
        }

        public DriverType DriverType { get; private set; }
    }
}