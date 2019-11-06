using System.ComponentModel;

namespace Fakka.Core.Enums
{
    public enum ContentType
    {
        [Description("application/json")]
        Json = 0,
        [Description("application/x-www-form-urlencoded")]
        Form = 1,
        [Description("multipart/form-data")]
        Multipart = 2

    }
}
