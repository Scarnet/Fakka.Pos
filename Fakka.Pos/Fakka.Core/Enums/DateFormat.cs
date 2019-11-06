using System.ComponentModel;

namespace Fakka.Core.Enums
{
    public enum DateFormat
    {
        [Description("yyyyMMddHHmmss")]
        DateTimeDefault = 0,
        
        [Description("yyyy/MM/dd-hh:mm:ss")]
        FullDateTime = 1,

        [Description("yyyy-MM-dd")]
        HijriDate = 2,

    }
}
