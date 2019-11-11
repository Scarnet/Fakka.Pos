using Fakka.Core.Utilities;
using FFImageLoading.Forms;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Fakka.Pos.Components.Children
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChildImage : ContentView
    {
        public static BindableProperty ChildNameProperty = BindableProperty.Create(nameof(ChildName), typeof(string), typeof(ChildImage), string.Empty, propertyChanged: HandleChildNameChanged);

        public static BindableProperty ParentNameProperty = BindableProperty.Create(nameof(ParentName), typeof(string), typeof(ChildImage), string.Empty, propertyChanged: HandleChildNameChanged);

        public static BindableProperty ChildImageSourceProperty = BindableProperty.Create(nameof(ChildImageSource), typeof(ImageSource), typeof(ChildImage), default(ImageSource), propertyChanged: HandleChildImageChanged);


        public static BindableProperty RadiusProperty = BindableProperty.Create(nameof(Radius), typeof(float), typeof(ChildImage), default(float), propertyChanged: HandleRadiusChanged);
        public static BindableProperty HasImageProperty = BindableProperty.Create(nameof(HasImage), typeof(bool), typeof(ChildImage), true, propertyChanged: HandleHasImageChanged);


        public static BindableProperty FontSizeProperty = BindableProperty.Create(nameof(FontSize), typeof(string), typeof(ChildImage), "Large", propertyChanged: HandleFontSizeChanged);
        public static BindableProperty TextColorProperty = BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(ChildImage), Color.Orange);
        public static BindableProperty BorderColorProperty = BindableProperty.Create(nameof(BorderColor), typeof(Color), typeof(ChildImage), Color.Orange);
        public string ChildName
        {
            get { return (string)GetValue(ChildNameProperty); }
            set { SetValue(ChildNameProperty, value); }
        }

        public string ParentName
        {
            get { return (string)GetValue(ParentNameProperty); }
            set { SetValue(ParentNameProperty, value); }
        }

        public ImageSource ChildImageSource
        {
            get { return (ImageSource)GetValue(ChildImageSourceProperty); }
            set { SetValue(ChildImageSourceProperty, value); }
        }
        public float Radius
        {
            get { return (float)GetValue(RadiusProperty); }
            set { SetValue(RadiusProperty, value); }
        }
        public bool HasImage
        {
            get { return (bool)GetValue(HasImageProperty); }
            set { SetValue(HasImageProperty, value); }
        }
        public string FontSize
        {
            get { return (string)GetValue(FontSizeProperty); }
            set { SetValue(FontSizeProperty, value); }
        }

        public Color BorderColor
        {
            get => (Color)GetValue(BorderColorProperty);
            set => SetValue(BorderColorProperty, value);
        }

        private float _borderWidth;
        public float BorderWidth
        {
            get => _borderWidth;
            set
            {
                _borderWidth = value;
                container.Padding = new Thickness(_borderWidth);
            }
        }

        public Color TextColor
        {
            get => (Color)GetValue(TextColorProperty);
            set => SetValue(TextColorProperty, value); 
        }

        public ChildImage()
        {
            InitializeComponent();
        }

        private static void HandleChildNameChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (ChildImage)bindable;

            var letter1 = control.ChildName.IsNullOrWhiteSpace() ? '\0' : control.ChildName.First();
            var letter2 = control.ParentName.IsNullOrWhiteSpace() ? '\0' : control.ParentName.First();
            control.InitialsLabel.Text = $"{letter1} {letter2}";
        }

        private static void HandleRadiusChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (ChildImage)bindable;

            double dimention = ((float)newValue) * 2;

            control.container.HeightRequest = dimention;
            control.container.WidthRequest = dimention;
            control.image.HeightRequest = dimention;
            control.image.WidthRequest = dimention;
            control.container.CornerRadius = (float)newValue;
        }

        private static void HandleChildImageChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (ChildImage)bindable;
            control.image.Source = (ImageSource)newValue;
        }
        private static void HandleFontSizeChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (ChildImage)bindable;
            double size;

            bool isValidDouble = double.TryParse(newValue.ToString(), out size);

            if (!isValidDouble)
                size = Device.GetNamedSize((NamedSize)System.Enum.Parse(typeof(NamedSize), newValue.ToString()), control.InitialsLabel.GetType());

            control.InitialsLabel.FontSize = size;
        }


        private static void HandleHasImageChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (ChildImage)bindable;
            bool hasImage = (bool)newValue;

            control.InitialsLabel.IsVisible = !hasImage;
            control.image.IsVisible = hasImage;

        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == ChildImageSourceProperty.PropertyName)
            {
                image.Source = ChildImageSource;
            }
        }

        private void Image_Error(object sender, CachedImageEvents.ErrorEventArgs e)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                if (ChildName.IsNullOrWhiteSpace() && ParentName.IsNullOrWhiteSpace())
                    return;

                InitialsLabel.IsVisible = true;
                image.IsVisible = false;

            });
        }
    }
}