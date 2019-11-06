using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CoreGraphics;
using Fakka.Pos.iOS.Effects;
using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using SharedEffect = Fakka.Core.Effects.ShadowEffect;

[assembly: ResolutionGroupName("Fakka")]
[assembly: ExportEffect(typeof(ShadowEffect), "ShodowEffect")]
namespace Fakka.Pos.iOS.Effects
{
    public class ShadowEffect : PlatformEffect
    {
        private double radius, distanceX, distanceY;
        UIView control;
        protected override void OnAttached()
        {
            radius = SharedEffect.GetRadius(Element);
            distanceX = SharedEffect.GetDistanceX(Element);
            distanceY = SharedEffect.GetDistanceY(Element);
            this.control = Control ?? Container;
            AddShadowLayer();
        }

        protected override void OnDetached()
        {
            Container.Layer.ShadowRadius =0;
            Container.Layer.ShadowOffset = new CGSize(0, 0);
        }

        private void AddShadowLayer()
        {
            Container.Layer.ShadowRadius = (float)this.radius;
            Container.Layer.ShadowOffset = new CGSize((float)this.distanceX, (float)this.distanceY);
        }
    }
}