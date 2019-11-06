using System;
using Fakka.Pos.Enum;
using Prism.Commands;
using Xamarin.Forms;

namespace Fakka.Pos.Renderers
{
    public class EntryExtension : Entry
    {
        public EntryExtension()
        {
        }

        public event EventHandler<Color> BorderColorChanged;
        public static readonly BindableProperty BorderColorProperty = BindableProperty.Create(
            nameof(BorderColor), typeof(Color), typeof(EntryExtension), Color.Default, BindingMode.TwoWay);

        public Color BorderColor
        {
            get => (Color)GetValue(BorderColorProperty);
            set
            {
                SetValue(BorderColorProperty, value);
                BorderColorChanged?.Invoke(this, value);
            }
        }


        public static readonly BindableProperty IconProperty = BindableProperty.Create(nameof(Icon), typeof(string), typeof(EntryExtension));

        public string Icon
        {
            get => (string)GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }

        public static readonly BindableProperty ImageAlignmentProperty =
            BindableProperty.Create(nameof(ImageAlignment), typeof(ImageAlignment), typeof(EntryExtension), ImageAlignment.Left);
        public ImageAlignment ImageAlignment

        {
            get { return (ImageAlignment)GetValue(ImageAlignmentProperty); }
            set { SetValue(ImageAlignmentProperty, value); }
        }

        public static readonly BindableProperty ImageHeightProperty = BindableProperty.Create(nameof(ImageHeight), typeof(int), typeof(EntryExtension), default(int));
        public int ImageHeight
        {
            get => (int)GetValue(ImageHeightProperty);
            set => SetValue(ImageHeightProperty, value);
        }

        public static readonly BindableProperty ImageWidthProperty = BindableProperty.Create(nameof(ImageWidthProperty), typeof(int), typeof(EntryExtension), default(int));
        public int ImageWidth
        {
            get => (int)GetValue(ImageWidthProperty);
            set => SetValue(ImageWidthProperty, value); 
        }

    }
}
