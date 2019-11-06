using Fakka.Pos.Models;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Fakka.Pos.Components.StockItems
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SelectedProductsList : ContentView
    {
        public static readonly BindableProperty ItemsProperty = 
            BindableProperty.Create(nameof(Items), typeof(ObservableCollection<LineItem>), typeof(SelectedProductsList), new ObservableCollection<LineItem>(), propertyChanged: HandleItemsChanged);

        public static readonly BindableProperty IncreasedCommandProperty = BindableProperty.Create(nameof(IncreasedCommand), typeof(DelegateCommand<object>), typeof(SelectedProductsList));
        public static readonly BindableProperty DecreasedCommandProperty = BindableProperty.Create(nameof(DecreasedCommand), typeof(DelegateCommand<object>), typeof(SelectedProductsList));
        public static readonly BindableProperty TotalPriceProperty = BindableProperty.Create(nameof(TotalPrice), typeof(decimal), typeof(SelectedProductsList), default(decimal));
        public ObservableCollection<LineItem> Items
        {
            get => (ObservableCollection<LineItem>)GetValue(ItemsProperty);
            set => SetValue(ItemsProperty, value);
        }

        public DelegateCommand<object> IncreasedCommand
        {
            get => (DelegateCommand<object>)GetValue(IncreasedCommandProperty);
            set => SetValue(IncreasedCommandProperty, value);
        }

        public DelegateCommand<object> DecreasedCommand
        {
            get => (DelegateCommand<object>)GetValue(DecreasedCommandProperty);
            set => SetValue(DecreasedCommandProperty, value);
        }

        public decimal TotalPrice
        {
            get => (decimal)GetValue(TotalPriceProperty);
            set => SetValue(TotalPriceProperty, value);
        }

        private static void HandleItemsChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (SelectedProductsList)bindable;
            var items = (ObservableCollection<LineItem>)newValue;

            if(oldValue != null)
            {
                var oldItems = (ObservableCollection<LineItem>)oldValue;
                oldItems.CollectionChanged -= control.Items_CollectionChanged;
            }
            control.TotalPrice = items?.Sum(i => (decimal)i.TotalPrice) ?? 0;

            if (items == null)
                return;

            items.CollectionChanged += control.Items_CollectionChanged;
            
        }


        public SelectedProductsList()
        {
            InitializeComponent();
        }

        private void Items_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {            
            TotalPrice = Items?.Sum(i => i.UnitPrice * i.Quantity) ?? 0;

            if(e.NewItems != null)
                foreach(LineItem item in e.NewItems)
                {
                    if (item == null)
                        continue;
                    item.PropertyChanged += Item_PropertyChanged;
                }

            if(e.OldItems != null)
                foreach (LineItem item in e.OldItems)
                {
                    if (item == null)
                        continue;
                    item.PropertyChanged -= Item_PropertyChanged;
                }

            ItemsList.ScrollTo(0, animate: true);
        }

        private void Item_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            TotalPrice = Items?.Sum(i => i.UnitPrice * i.Quantity) ?? 0;

        }
    }
}