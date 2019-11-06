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

        public static BindableProperty ItemsProperty = BindableProperty.Create(nameof(Items), typeof(ICollection<StockItem>), typeof(StockItemRepeater), new Collection<StockItem>(), propertyChanged: HandleItemChanged);

        public ICollection<StockItem> Items
        {
            get { return (ICollection<StockItem>)GetValue(ItemsProperty); }
            set { SetValue(ItemsProperty, value); }
        }

        private static void HandleItemChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var translator = new TranslateExtension();
            var control = (StockItemRepeater)bindable;
            var items = (ICollection<StockItem>)newValue;
            control.StockItemContainer.Children.Clear();

            if (items == null)
                return;

            var groupedItems = items.GroupBy(item => item.Type);

            items = items.OrderBy(i => i.Type).ToList();
            //foreach (var groupedItem in groupedItems) 
            //{
            //var groupHeader = new StockItemsGroupHeader() { Margin = new Thickness(0, 10, 0, 0) };

            //translator.Text = groupedItem.Key.ToString();
            //groupHeader.GroupName = translator.ProvideValue(null).ToString();

            //control.StockItemContainer.Children.Add(groupHeader);

            StackLayout groupContainer = null;

            //int itemsCount = groupedItem.Count();
            int itemsCount = items.Count;
            var horizonatlOptions = LayoutOptions.CenterAndExpand;
            for (int i = 0; i < itemsCount; i++)
            {
                //var item = groupedItem.ElementAt(i);
                var item = items.ElementAt(i);
                if (i % 7 == 0)
                {
                    groupContainer = new StackLayout
                    {
                        Orientation = StackOrientation.Horizontal,
                        BackgroundColor = (Color)App.Current.Resources["LightGreyColor"],
                    };

                    if (i == 0)
                        groupContainer.Padding = new Thickness(2, 7);

                    control.StockItemContainer.Children.Add(groupContainer);
                    horizonatlOptions = itemsCount - i < 6 ? LayoutOptions.Start : LayoutOptions.CenterAndExpand; //layout cards to the start if the remaning items in the group is less than 5
                }

                if (item.Type == ItemType.Product)
                {
                    var card = new StockItemCard() { HorizontalOptions = LayoutOptions.CenterAndExpand, VerticalOptions = LayoutOptions.Center };
                    var tapGesture = new TapGestureRecognizer();
                    tapGesture.Tapped += (s, e) => control.HandleStockItemCardSelected(card, item);

                    card.GestureRecognizers.Add(tapGesture);

                    card.ImageRadius = 62;
                    card.SetBinding(StockItemCard.ItemNameProperty, new Binding("Name", source: item));
                    card.SetBinding(StockItemCard.ItemQtyProperty, new Binding("Quantity", source: item));
                    card.SetBinding(StockItemCard.ItemPriceProperty, new Binding("UnitPrice", source: item));
                    card.SetBinding(StockItemCard.IsBannedProperty, new Binding("IsBanned", source: item));
                    if (item.LocalImage != null && item.LocalImage.Length > 2)
                        card.SetBinding(StockItemCard.ImageProperty, new Binding("LocalImage", source: item, converter: new BytesToImageConverter()));
                    else
                        card.SetBinding(StockItemCard.ImageProperty, new Binding("Image", source: item));

                    groupContainer.Children.Add(card);

                }
                else
                {
                    var card = new MealCard() { HorizontalOptions = LayoutOptions.CenterAndExpand, VerticalOptions = LayoutOptions.Center };
                    var tapGesture = new TapGestureRecognizer();
                    tapGesture.Tapped += (s, e) => control.HandleMealCardSelected(card, item);

                    card.GestureRecognizers.Add(tapGesture);

                    card.ImageRadius = 62;
                    card.SetBinding(MealCard.ItemNameProperty, new Binding("Name", source: item));
                    card.SetBinding(MealCard.ItemPriceProperty, new Binding("UnitPrice", source: item));
                    if (item.LocalImage != null && item.LocalImage.Length > 2)
                        card.SetBinding(MealCard.ImageProperty, new Binding("LocalImage", source: item, converter: new BytesToImageConverter()));
                    else
                        card.SetBinding(MealCard.ImageProperty, new Binding("Image", source: item));

                    groupContainer.Children.Add(card);
                }
            }
            //}

            control.UpdateChildrenLayout();
        }

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

    }
}