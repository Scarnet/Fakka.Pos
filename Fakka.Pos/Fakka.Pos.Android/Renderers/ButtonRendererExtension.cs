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
using Fakka.Pos.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Xamarin.Forms.Button), typeof(ButtonRendererExtension))]
namespace Fakka.Pos.Droid.Renderers
{
    public class ButtonRendererExtension : ButtonRenderer
    {
        public ButtonRendererExtension(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Button> e)
        {
            base.OnElementChanged(e);

            Control.Gravity = GravityFlags.CenterVertical | GravityFlags.CenterHorizontal;
            //Control.SetPadding(0, 0, 0, 0);
        }
    }
}