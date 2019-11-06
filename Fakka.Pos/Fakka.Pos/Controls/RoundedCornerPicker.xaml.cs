using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Fakka.Pos.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RoundedCornerPicker : ContentView
    {
        public readonly static BindableProperty ItemsProperty = BindableProperty.Create(nameof(Items), typeof(IList), typeof(RoundedCornerPicker), propertyChanged: HandleItemsChanged);
        public readonly static BindableProperty SelectedItemProperty = BindableProperty.Create(nameof(SelectedItem), typeof(object),typeof(RoundedCornerPicker), propertyChanged: HandleItemSelectedChanged);
        public readonly static BindableProperty CornerRadiusProperty = BindableProperty.Create(nameof(CornerRadius), typeof(float), typeof(RoundedCornerPicker), default(float), propertyChanged: HandleCornerRadiusChanged);

        public IList Items
        {
            get => (IList)GetValue(ItemsProperty);
            set => SetValue(ItemsProperty, value);
        }

        public object SelectedItem
        {
            get => GetValue(SelectedItemProperty);
            set => SetValue(SelectedItemProperty, value);
        }

        public string Title
        {
            get => picker.Title;
            set => picker.Title = value;
        }

        public BindingBase ItemDisplayName
        {
            get => picker.ItemDisplayBinding;
            set => picker.ItemDisplayBinding = value;
        }

        public float CornerRadius
        {
            get => (float)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }
        
        public Color BorderColor
        {
            get => Container.BorderColor;
            set => Container.BorderColor = value;
        }
        public Color PickerBackgroundColor
        {
            get => Container.BackgroundColor;
            set => Container.BackgroundColor = value;
        }


        private static void HandleItemsChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (RoundedCornerPicker)bindable;
            control.picker.ItemsSource = (IList)newValue;
        }

        private static void HandleItemSelectedChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (RoundedCornerPicker)bindable;
            control.SelectedItem = newValue;
            //control.SelectedText = control.picker.co
        }

        private static void HandleCornerRadiusChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (RoundedCornerPicker)bindable;
            float radius = (float)newValue;
            control.Container.CornerRadius = radius;
            control.Container.HeightRequest = radius * 2;
        }


        public RoundedCornerPicker()
        {
            InitializeComponent();
            BindingContext = this;
        }

        private void OnPickerContainerTapped(object sender, EventArgs e)
        {
            picker.Focus();
        }
    }
}