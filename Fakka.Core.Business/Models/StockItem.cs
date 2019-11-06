using Fakka.Core.Business.Enums;
using Fakka.Core.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fakka.Core.Business.Models
{
    public class StockItem : BaseStockItem
    {
        public string SchoolId { get; set; }
        public ItemType Type { get; set; } = ItemType.Product;
        public string SubCategoryId { get; set; }
        public string SubCategoryName { get; set; }
        public Status Status { get; set; }
        private bool _isSelected;
        [JsonIgnore]
        public bool IsSelected
        {
            get => _isSelected;
            set => SetProperty(ref _isSelected, value);
        }

        private bool _isBanned;
        [JsonIgnore]
        public bool IsBanned
        {
            get => _isBanned;
            set => SetProperty(ref _isBanned, value);
        }

        public string Image
        {
            get { return ImageHandler.GetImageUri(this.Id, this.Type == ItemType.Product ? "products" : $"meals/{this.SchoolId}"); }
        }

        private byte[] localImage;
        public byte[] LocalImage
        {
            get => localImage;
            set
            {
                localImage = value;
                RaisePropertyChanged();
            }
        }

    }
}
