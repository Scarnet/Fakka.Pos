using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Fakka.Pos.Components.StockItems
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MealCard : ContentView
    {
        public static BindableProperty ImageRadiusProperty = BindableProperty.Create(nameof(ImageRadius), typeof(float), typeof(StockItemCard), default(float), propertyChanged: HandleRadiusChanged);
        public static BindableProperty ItemNameProperty = BindableProperty.Create(nameof(ItemName), typeof(string), typeof(StockItemCard), string.Empty);
        public static BindableProperty ItemPriceProperty = BindableProperty.Create(nameof(ItemPrice), typeof(float), typeof(StockItemCard), default(float));
        public static BindableProperty ImageProperty = BindableProperty.Create(nameof(Image), typeof(ImageSource), typeof(StockItemCard), default(ImageSource));
        public readonly static BindableProperty IsSelectedProperty = BindableProperty.Create(nameof(IsSelected), typeof(bool), typeof(StockItemCard), false);
        public float ImageRadius
        {
            get { return (float)GetValue(ImageRadiusProperty); }
            set { SetValue(ImageRadiusProperty, value); }
        }

        public string ItemName
        {
            get { return (string)GetValue(ItemNameProperty); }
            set { SetValue(ItemNameProperty, value); }
        }

        public float ItemPrice
        {
            get { return (float)GetValue(ItemPriceProperty); }
            set { SetValue(ItemPriceProperty, value); }
        }

        public ImageSource Image
        {
            get { return (ImageSource)GetValue(ImageProperty); }
            set { SetValue(ImageProperty, value); }
        }
        public bool IsSelected
        {
            get { return (bool)GetValue(IsSelectedProperty); }
            set { SetValue(IsSelectedProperty, value); }
        }


        private static void HandleRadiusChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (MealCard)bindable;

            float radius = (float)newValue;
            control.ImageContainer.HeightRequest = radius;
            control.ImageContainer.WidthRequest = radius;
            control.ImageContainer.CornerRadius = radius;

            control.Footer.HeightRequest = radius * 0.7;
            control.Footer.WidthRequest = radius;
            control.UpdateChildrenLayout();
        }

        public MealCard()
        {
            InitializeComponent();
        }
    }
}