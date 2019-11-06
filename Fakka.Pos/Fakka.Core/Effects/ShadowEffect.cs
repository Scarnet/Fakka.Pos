using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Fakka.Core.Effects
{
    public class ShadowEffect
    {
        public static readonly BindableProperty RadiusProperty =
          BindableProperty.CreateAttached("Radius", typeof(double), typeof(ShadowEffect), 1.0);
        public static readonly BindableProperty DistanceXProperty =
          BindableProperty.CreateAttached("DistanceX", typeof(double), typeof(ShadowEffect), 0.0);
        public static readonly BindableProperty DistanceYProperty =
          BindableProperty.CreateAttached("DistanceY", typeof(double), typeof(ShadowEffect), 0.0);



        public static double GetRadius(BindableObject view)
        {
            return (double)view.GetValue(RadiusProperty);
        }

        public static double GetDistanceX(BindableObject view)
        {
            return (double)view.GetValue(DistanceXProperty);
        }

        public static double GetDistanceY(BindableObject view)
        {
            return (double)view.GetValue(DistanceYProperty);
        }

    }
}
