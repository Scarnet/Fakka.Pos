using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Fakka.Pos.Templates
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StockItemTemplate : ContentView
    {
        public static BindableProperty ImageRadiusProperty = BindableProperty.Create(nameof(ImageRadius), typeof(float), typeof(StockItemTemplate), default(float), propertyChanged: HandleRadiusChanged);

        public float ImageRadius
        {
            get { return (float)GetValue(ImageRadiusProperty); }
            set { SetValue(ImageRadiusProperty, value); }
        }

        private static void HandleRadiusChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (StockItemTemplate)bindable;

            float radius = (float)newValue;
            control.ImageContainer.HeightRequest = radius;
            control.ImageContainer.WidthRequest = radius;
            control.ImageContainer.CornerRadius = radius;

            control.Footer.HeightRequest = radius * 0.7;
            control.Footer.WidthRequest = radius;
            control.UpdateChildrenLayout();
        }

        public StockItemTemplate()
        {
            InitializeComponent();
        }


    }
}