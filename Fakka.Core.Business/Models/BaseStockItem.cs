using System;
using System.Collections.Generic;
using System.Text;

namespace Fakka.Core.Business.Models
{
    public class BaseStockItem : BaseModels
    {

        public string Name { get; set; }

        private int _quantity;
        public int Quantity { get => _quantity; set => SetProperty(ref _quantity, value); }
        public decimal UnitPrice { get; set; }

    }
}
