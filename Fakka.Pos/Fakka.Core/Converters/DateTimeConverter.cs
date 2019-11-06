using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Fakka.Core.Enums;
using Fakka.Core.Extensions;
using Fakka.Core.Utilities;
using Xamarin.Forms;

namespace Fakka.Core.Converters
{

    public class DateTimeConverter : IValueConverter
    {
        /// <summary>
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {

                if (value is DateTime)
                {
                    CultureInfo arSA = new CultureInfo(CultureLocale.ArKsa.ToDescriptionString());
                    // arSA.DateTimeFormat.Calendar = new HijriCalendar();
                    return (value as DateTime?)?.ToString(parameter as string, arSA);
                    //,DateTime.ParseExact(value as string, "yyyy-MM-ddThh:mm:ssZ", arSA);
                    //return (new DateTimeHandler(date, parameter as string, CultureLocale.ArKsa).Display);
                }

                if (value is DateTimeHandler)
                {
                    return ((DateTimeHandler) value).Display;
                }
                return value;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

    

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
