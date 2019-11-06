using Fakka.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fakka.Config.Settings
{
    public static class AuthenticationSettings
    {
        // Identity Configuration
        public const string ClientId = "66c76936-7aa5-c705-adcd-08d4a025fe57";
        public const string Scope = "";
        public const string ClientSecret = "test1234";
        public const string GrantType = "password";
        public static UserRole[] ValidUserRoles = { UserRole.PointOfSaleUser };
    }
}
