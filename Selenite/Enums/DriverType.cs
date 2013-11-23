using System.ComponentModel;

namespace Selenite.Enums
{
    public enum DriverType
    {
        [Description("Chrome")]
        Chrome,

        [Description("Firefox")]
        Firefox,

        [Description("Internet Explorer")]
        InternetExplorer,

        [Description("PhantomJS")]
        PhantomJs,

        [Description("Web Client")]
        WebClient,

        Unknown
    }
}