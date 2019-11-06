using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace Fakka.Pos.Converter
{
    public class BoolToSelectionColorConverter : IValueConverter
    {
        public Color SelectedColor { get; set; }
        public Color UnselectedColor { get; set; }
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var selected = (bool)value;

            return selected ? SelectedColor : UnselectedColor;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var color = (Color)value;

            return color == SelectedColor ? true : false;
        }
    }
}
