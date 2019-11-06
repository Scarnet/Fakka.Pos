using System.ComponentModel;

namespace Fakka.Core.Enums
{
    public enum UserRole
    {
        [Description("None")]
        None = 0,
        [Description("Parent")]
        Parent = 1,
        [Description("SchoolAdmin")]
        SchoolAdmin,
        [Description("POSUser")]
        PointOfSaleUser,
        [Description("SystemAdmin")]
        FakkaAdmin
    }
}