using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Fakka.Pos.Droid.Effects;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using SharedEffect = Fakka.Core.Effects.ShadowEffect;
using View = Android.Views.View;

[assembly: ResolutionGroupName("Fakka")]
[assembly: ExportEffect(typeof(ShadowEffect), "ShadowEffect")]
namespace Fakka.Pos.Droid.Effects
{

    public class ShadowEffect : PlatformEffect
    {
        private double radius, distanceX, distanceY;
        View control;
        protected override void OnAttached()
        {
            radius = SharedEffect.GetRadius(Element);
            distanceX = SharedEffect.GetDistanceX(Element);
            distanceY = SharedEffect.GetDistanceY(Element);
            this.control = Control ?? Container as View;

            UpdateShadow();
        }

        protected override void OnDetached()
        {
            this.control.Elevation = 0;
            this.control.TranslationZ = 0;
        }

        private void UpdateShadow()
        {
            try
            {
                this.control = Control ?? Container as View;

                float radius = (float)this.radius;

                this.control.Elevation = radius;
                this.control.TranslationZ = (float)(this.distanceX + this.distanceY) / 2;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Cannot set property on attached control. Error: {0}", ex.Message);
            }
        }

    }
}