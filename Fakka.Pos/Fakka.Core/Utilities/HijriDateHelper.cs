using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Fakka.Core.Utilities
{
    public static class HijriDateHelper
    {
        public static DateTime AddHijriYears(this DateTime date, int years)
        {
            var arSA = new CultureInfo("ar-SA");

            string[] dateParts = date.ToString("yyyy-MM-dd", arSA).Split('-');

            int hijriYear = int.Parse(dateParts[0]);
            int hijriMonth = int.Parse(dateParts[1]);
            int hijriDay = int.Parse(dateParts[2]);

            int newHijriYear = hijriYear + years;

            return new DateTime(newHijriYear, hijriMonth, hijriDay, new UmAlQuraCalendar());
        }

    }
}
