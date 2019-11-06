using Fakka.Core.Business.Enums;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fakka.Pos.Models
{
    public class LineItem : BaseUiModel
    {

        public string Id { get; set; }
        public string Name { get; set; }
        public decimal UnitPrice { get; set; }
        public ItemType ItemType { get; set; }

        private int _quantity = 1;
        public int Quantity
        {
            get => _quantity;
            set
            {
                SetProperty(ref _quantity, value);
                RaisePropertyChanged(nameof(TotalPrice));
            }
        }

        public int MaxQuantity { get; set; }

        public float TotalPrice => (float)UnitPrice * Quantity;
    }
}
