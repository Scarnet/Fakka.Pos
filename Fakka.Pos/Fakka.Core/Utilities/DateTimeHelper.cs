using System;
using System.Collections.Generic;
using System.Text;

namespace Fakka.Core.Utilities
{
    public static class DateTimeHelper
    {
        public static string ToEdm(this DateTime date)
        {
            return date.ToString("yyyy-MM-ddTHH:mm:ssZ");
        }
    }
}
