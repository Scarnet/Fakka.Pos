using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Fakka.Core.Utilities;
using Xamarin.Forms;

namespace Fakka.Core.Converters
{

    public class Base64ToImageConverter : IValueConverter
    {
        /// <summary>
        /// value is the base64 image string
        /// pararmeter is the default image placeholder
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ImageHandler.Base64ToImageSource(value as string, parameter as string);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
