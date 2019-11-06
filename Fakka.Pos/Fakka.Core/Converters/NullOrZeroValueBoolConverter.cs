using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Fakka.Core.Converters
{
    public class NullOrZeroValueBoolConverter : IValueConverter , IMarkupExtension
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            if (value is string)
            {
                if (string.IsNullOrEmpty(value as string))
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {

                if (value == null)
                {
                    return false;
                }
                else if ((decimal)value == 0)
                {

                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

    }
}
