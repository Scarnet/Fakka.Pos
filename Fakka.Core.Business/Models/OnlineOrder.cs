using Fakka.Core.JsonConverters;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Fakka.Core.Utilities;

namespace Fakka.Core.Business.Models
{
    public class OnlineOrder : BaseModels
    {
        public string Name { get; set; }
        public string ChildId { get; set; }
        public int ChildCode { get; set; }
        public string ChildName { get; set; }
        public string ParentName { get; set; }
        public string ParentId { get; set; }
        public string ChildImage
        {
            get => ImageHandler.GetImageUri(this.ChildId, $"children/{this.ParentId}");
        }
        public decimal Total => OrderItems?.Sum(item => item.Quantity * item.UnitPrice) ?? 0;
        public DateTime DurationFrom { get; set; }
        public DateTime DurationTo { get; set; }
        public bool ChildHasImage { get; set; }
        public string HijriDurationFrom { get; set; }
        public string HijriDurationTo { get; set; }
        [JsonConverter(typeof(JsonStringConverter))]
        public List<TransactionItem> OrderItems { get; set; }
    }
}
 