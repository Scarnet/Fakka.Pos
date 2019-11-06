using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Fakka.Pos.Controls;
using Fakka.Pos.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(ExtendedPicker), typeof(ExtendedPickerRenderer))]
namespace Fakka.Pos.Droid.Renderers
{
    public class ExtendedPickerRenderer : PickerRenderer
    {
        private ExtendedPicker element;
        public ExtendedPickerRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Picker> e)
        {
            base.OnElementChanged(e);

            this.element = (ExtendedPicker)e.NewElement;

            var color = this.element.UnderlineColor;
            Control.BackgroundTintList = ColorStateList.ValueOf(color.ToAndroid());
        }

    }
}