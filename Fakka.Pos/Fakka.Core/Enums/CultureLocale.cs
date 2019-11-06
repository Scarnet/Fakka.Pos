using System.ComponentModel;

namespace Fakka.Core.Enums
{
    public enum CultureLocale
    {
        [Description("invariant")]
        Invariant = 0,
        [Description("en-US")]
        EngUs = 1,
        [Description("ar-EG")]
        ArEgy = 2,
        [Description("ar-SA")]
        ArKsa = 3
    }
}