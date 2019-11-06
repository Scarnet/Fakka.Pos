using Fakka.Core.Business.Enums;
using Fakka.Core.Business.Models;
using Fakka.Core.Converters;
using Fakka.Pos.Components.StockItems;
using Fakka.Pos.EventArguments;
using Fakka.Pos.Resources;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Fakka.Pos.Components
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StockItemRepeater : ContentView
    {
        public event EventHandler<StockItem> Selected;

        public static BindableProperty ItemsProperty = BindableProperty.Create(nameof(Items), typeof(ICollection<StockItem>), typeof(StockItemRepeater), new Collection<StockItem>());

        public ICollection<StockItem> Items
        {
            get { return (ICollection<StockItem>)GetValue(ItemsProperty); }
            set { SetValue(ItemsProperty, value); }
        }

        //private static void HandleItemChanged(BindableObject bindable, object oldValue, object newValue)
        //{
            
        //    var control = (StockItemRepeater)bindable;

        //    var items = (ICollection<StockItem>)newValue;

        //    items = items?.OrderBy(item => item.Type).ToList();

        //    control.ItemsList.ItemsSource = items;
        //}

        private void HandleStockItemCardSelected(StockItemCard card, StockItem item)
        {
            bool selected = !card.IsSelected;


            card.SetBinding(StockItemCard.IsSelectedProperty, new Binding("IsSelected", BindingMode.TwoWay, source: item));
            item.IsSelected = selected;
            Selected?.Invoke(card, item);

        }

        private void HandleMealCardSelected(MealCard card, StockItem item)
        {
            bool selected = !card.IsSelected;


            card.SetBinding(MealCard.IsSelectedProperty, new Binding("IsSelected", BindingMode.TwoWay, source: item));
            item.IsSelected = selected;
            Selected?.Invoke(card, item);
        }

        public StockItemRepeater()
        {
            InitializeComponent();
        }

        private void ItemsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!e.CurrentSelection.Any())
                return;

            var lst = (CollectionView)sender;
            var stockItem = (StockItem)e.CurrentSelection[0];

            stockItem.IsSelected = !stockItem.IsSelected;
            Selected?.Invoke(sender, stockItem);

            lst.SelectedItem = null;
        }
    }
}