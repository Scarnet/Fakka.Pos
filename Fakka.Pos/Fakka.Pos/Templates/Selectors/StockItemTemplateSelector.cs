using Fakka.Core.Business.Enums;
using Fakka.Core.Business.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Fakka.Pos.Templates.Selectors
{
    public class StockItemTemplateSelector : DataTemplateSelector
    {
        public DataTemplate ProductTemplate { get; set; }
        public DataTemplate MealTemplate { get; set; }
        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var stockItem = (StockItem)item;

            return stockItem.Type == ItemType.Product ? ProductTemplate : MealTemplate;
        }
    }
}
