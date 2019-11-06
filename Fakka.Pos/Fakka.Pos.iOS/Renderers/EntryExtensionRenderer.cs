using CoreAnimation;
using CoreGraphics;
using Fakka.Core.Managers;
using Fakka.IOS.Renderers;
using Fakka.Pos.Enum;
using Fakka.Pos.Renderers;
using System;
using System.Drawing;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(EntryExtension), typeof(EntryExtensionRenderer))]

namespace Fakka.IOS.Renderers
{
    public class EntryExtensionRenderer : EntryRenderer
    {

        private EntryExtension element;

        public override bool CanPerform(ObjCRuntime.Selector action, Foundation.NSObject withSender)
        {
            if ((action == new ObjCRuntime.Selector("paste:"))
                || (action == new ObjCRuntime.Selector("copy:"))
                || (action == new ObjCRuntime.Selector("cut:"))
                )
            {
                return false;
            }

            return base.CanPerform(action, withSender);
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);
            this.element = (EntryExtension)e.NewElement;
            if (Control != null && e.NewElement != null && Element != null)
            {

                //  https://kampeki-factory.blogspot.com.eg/2017/06/xamarin-forms-custom-bottom-bordered.html
                // Need to connect to Sizechanged event because first render time, Entry has no size (-1).

                //if (e.NewElement != null)
                if (e.OldElement != null)
                    e.OldElement.SizeChanged -= HandleSizeChanged;

                e.NewElement.SizeChanged += HandleSizeChanged;
                

                //}

                SetupImageControl();

            }
        }

        private void HandleSizeChanged(object obj, object args)
        {
            var entry = obj as EntryExtension;
            if (entry == null)
                return;

            // Create borders (bottom only)
            CALayer border = new CALayer();
            float width = 1.0f;
            //border.BorderColor = new CoreGraphics.CGColor(0.73f, 0.7451f, 0.7647f);  // gray border color
            border.BorderColor = new CGColor(Control.TextColor.CGColor.Handle); // take the Text color
            border.Frame = new CGRect(x: 0, y: entry.Height + width, width: entry.Width, height: 1.0f);
            border.BorderWidth = width;

            Control.Layer.AddSublayer(border);

            //Adjust Text Alignment
            Control.TextAlignment = entry.HorizontalTextAlignment == TextAlignment.Center ? UITextAlignment.Center :
            ApplicationManager.Instance.GetApplicationInfo().IsRtl && entry.HorizontalTextAlignment == TextAlignment.Start ? UITextAlignment.Right : UITextAlignment.Left;
            Control.Layer.MasksToBounds = true;

            //Remove Borders
            Control.BorderStyle = UITextBorderStyle.None;
            Control.BackgroundColor = new UIColor(255, 255, 255, 0); // white

        }

        private void SetupImageControl()
        {
            if (!string.IsNullOrEmpty(this.element.Icon))
            {
                switch (this.element.ImageAlignment)
                {

                    case ImageAlignment.Left:
                        Control.LeftViewMode = UITextFieldViewMode.Always;
                        Control.LeftView = GetImageView(this.element.Icon, element.ImageHeight, element.ImageWidth);
                        break;

                    case ImageAlignment.Right:
                        Control.RightViewMode = UITextFieldViewMode.Always;
                        Control.RightView = GetImageView(this.element.Icon, element.ImageHeight, element.ImageWidth);
                        break;

                }

            }

        }

        private UIView GetImageView(string imagePath, int height, int width)

        {

            var uiImageView = new UIImageView(UIImage.FromBundle(imagePath))

            {
                Frame = new RectangleF(0, 0, width, height)
            };

            UIView objLeftView = new UIView(new System.Drawing.Rectangle(0, 0, width + 10, height));

            objLeftView.AddSubview(uiImageView);

            return objLeftView;

        }

    }

}
