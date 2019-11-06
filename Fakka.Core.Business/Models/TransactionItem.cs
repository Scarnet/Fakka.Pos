using Fakka.Core.Business.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fakka.Core.Business.Models
{
    public class TransactionItem : BaseModels
    {
        public string Name { get; set; }
        public Guid ItemId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public Guid TransactionId { get; set; }
        public ItemType ItemType { get; set; }

    }
}
