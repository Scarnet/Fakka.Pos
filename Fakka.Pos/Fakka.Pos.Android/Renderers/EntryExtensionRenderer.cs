using System;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Runtime;
using Android.Support.V4.Content;
using Android.Views;
using Fakka.Core.Managers;
using Fakka.Droid.Renderers;
using Fakka.Pos.Enum;
using Fakka.Pos.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Color = Xamarin.Forms.Color;

[assembly: ExportRenderer(typeof(EntryExtension), typeof(EntryExtensionRenderer))]

namespace Fakka.Droid.Renderers
{
    public class Callback : Java.Lang.Object, ActionMode.ICallback
    {
        public bool OnActionItemClicked(ActionMode mode, IMenuItem item)
        {
            return false;
        }

        public bool OnCreateActionMode(ActionMode mode, IMenu menu)
        {
            // Cut - 16908320
            // Copy - 16908321
            // Paste - 16908322
            // Share - 16908341

            if (menu != null)
            {
                menu.RemoveItem(16908322); // paste - 1020022 hex
                menu.RemoveItem(16908320); // cut - 1020020 hex
                menu.RemoveItem(16908321); // copy - 1020021 hex
                menu.RemoveItem(16908341); // share - 1020042 hex
            }

            return true;
        }

        public void OnDestroyActionMode(ActionMode mode)
        {
        }

        public bool OnPrepareActionMode(ActionMode mode, IMenu menu)
        {
            return false;
        }
    }
    public class EntryExtensionRenderer : EntryRenderer
    {

        private EntryExtension element;
        public EntryExtensionRenderer(Context context) : base(context)
        {

        }

        private void Element_BorderColorChanged(object sender, Color e)
        {

            if (this.element == null) return;

            var borderColor = Color.Default;

            if (this.element.BorderColor == Color.Default)
            {
                borderColor = string.IsNullOrEmpty(this.element.Text) ? this.element.PlaceholderColor : this.element.TextColor;
            }
            else
            {
                borderColor = e;
            }

            Control.Background.Mutate().SetColorFilter(borderColor.ToAndroid(),
                global::Android.Graphics.PorterDuff.Mode.SrcAtop);
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            this.element = (EntryExtension)Element;

            Control.CustomSelectionActionModeCallback = new Callback();

            if (Control != null && e.NewElement != null && Element != null)
            {
                ((EntryExtension)Element).BorderColorChanged += Element_BorderColorChanged;

                Control.Background.Mutate().SetColorFilter(e.NewElement.PlaceholderColor.ToAndroid(),
                    global::Android.Graphics.PorterDuff.Mode.SrcAtop);

                IntPtr IntPtrtextViewClass = JNIEnv.FindClass(Control.GetType());
                IntPtr mCursorDrawableResProperty = JNIEnv.GetFieldID(IntPtrtextViewClass, "mCursorDrawableRes", "I");

                JNIEnv.SetField(Control.Handle, mCursorDrawableResProperty, Fakka.Pos.Droid.Resource.Drawable.white_color_resource); 
                
                Control.Gravity = ApplicationManager.Instance.GetApplicationInfo().IsRtl
                    ? GravityFlags.Right
                    : GravityFlags.Left;

                SetupImageControl();


            }

        }

        private void SetupImageControl()
        {

            if (!string.IsNullOrEmpty(element.Icon))

            {
                switch (element.ImageAlignment)

                {

                    case ImageAlignment.Left:
                        Control.SetCompoundDrawablesWithIntrinsicBounds(GetDrawable(element.Icon), null, null, null);
                        break;
                    case ImageAlignment.Top:
                        Control.SetCompoundDrawablesWithIntrinsicBounds(null, GetDrawable(element.Icon), null, null);
                        break;
                    case ImageAlignment.Right:
                        Control.SetCompoundDrawablesWithIntrinsicBounds(null, null, GetDrawable(element.Icon), null);
                        break;
                    case ImageAlignment.Bottom:
                        Control.SetCompoundDrawablesWithIntrinsicBounds(null, null, null, GetDrawable(element.Icon));
                        break;
                }

            }

            //Control.CompoundDrawablePadding = 25;
        }



        private BitmapDrawable GetDrawable(string imageEntryImage)

        {

            int resID = Resources.GetIdentifier(imageEntryImage, "drawable", this.Context.PackageName);

            var drawable = ContextCompat.GetDrawable(this.Context, resID);

            var bitmap = ((BitmapDrawable)drawable).Bitmap;


            if (this.element.ImageWidth > 0 && this.element.ImageHeight > 0)
                return new BitmapDrawable(Resources, Bitmap.CreateScaledBitmap(bitmap, this.element.ImageWidth * 2, this.element.ImageHeight * 2, true));
            else
                return new BitmapDrawable(Resources, bitmap);
        }
    }
}
