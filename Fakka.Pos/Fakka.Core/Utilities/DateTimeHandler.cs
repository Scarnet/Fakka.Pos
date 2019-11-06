using System;
using System.Globalization;
using Fakka.Core.Enums;
using Fakka.Core.Extensions;

namespace Fakka.Core.Utilities
{
    public class DateTimeHandler
    {
        public readonly string Value; // yyyyMMddHHmmss fixed
        public readonly string Display; // output format
        public readonly DateTime Date;

        public DateTimeHandler() : this(DateTime.Now) { }
        public DateTimeHandler(DateFormat dateFormatOut, CultureLocale cultureLocale = CultureLocale.Invariant) : this(DateTime.Now, dateFormatOut, cultureLocale) { }
        public DateTimeHandler(DateTime datetime, DateFormat dateFormatOut = DateFormat.FullDateTime, CultureLocale cultureLocale = CultureLocale.Invariant) : this (datetime, dateFormatOut.ToDescriptionString(), cultureLocale) { }
        public DateTimeHandler(DateTime datetime, string dateFormatOut, CultureLocale cultureLocale)
        {
            var cultureInfoLocale = cultureLocale == CultureLocale.Invariant ? CultureInfo.InvariantCulture : new CultureInfo(cultureLocale.ToDescriptionString());
            Display = datetime.ToString(dateFormatOut, cultureInfoLocale);
            Value = datetime.ToString(DateFormat.DateTimeDefault.ToDescriptionString());
            Date = datetime;
        }



        public DateTimeHandler(string dateTimeStr, DateFormat dateFormatIn = DateFormat.DateTimeDefault, DateFormat dateFormatOut = DateFormat.FullDateTime)
        {
            // need to convert inputString to datetimeHandler object
            Date = ToDateTime(dateTimeStr, dateFormatIn);

            var dateTime = new DateTimeHandler(Date, dateFormatOut);
            Display = dateTime.Display;
            Value = dateTime.Value;
        }

        private DateTime ToDateTime(string dateTimeStr, DateFormat dateFormatIn, CultureLocale cultureLocale = CultureLocale.Invariant)
        {
            try
            {

                var cultureInfoLocale = cultureLocale == CultureLocale.Invariant ? CultureInfo.InvariantCulture : new CultureInfo(cultureLocale.ToDescriptionString());

                return DateTime.ParseExact(dateTimeStr, dateFormatIn.ToDescriptionString(), cultureInfoLocale);
            }
            catch (ArgumentNullException)
            {
                return DateTime.Now;
            }
            catch (FormatException)
            {
                return DateTime.Now;
            }

        }




    }
}
