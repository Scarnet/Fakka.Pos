using System;
using System.Collections.Generic;
using System.Text;

namespace Fakka.Core.Utilities
{
    public static class StringUtilities
    {
        public static bool IsNullOrWhiteSpace(this string str)
        {
            return string.IsNullOrWhiteSpace(str);
        }
    }
}
