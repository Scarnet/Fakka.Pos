using Fakka.Pos.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Fakka.Pos.Templates.Selectors
{
    public class TodayOrdersTemplateSelector : DataTemplateSelector
    {
        public DataTemplate OrderTemplate { get; set; }
        public DataTemplate OrderItemTemplate { get; set; }
        public DataTemplate EmptyTemplate { get; set; }
        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            if (item is TodayOnlineOrder)
                return OrderTemplate;
            else if (item is OrderTransaction orderItem)
            {
                if (orderItem.Quantity <= 0)
                    return EmptyTemplate;

                return OrderItemTemplate;
            }

            throw new NotSupportedException($"type {item.GetType().FullName} is not supported as an item");
        }
    }
}
