using Fakka.Core.Business.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fakka.Pos.Models
{
    public class TodayOnlineOrder : BaseUiModel
    {
        public string ChildId { get; set; }
        public int ChildCode { get; set; }
        public string Name { get; set; }
        public decimal Total { get; set; }
        public string HijriDurationFrom { get; set; }
        public string HijriDurationTo { get; set; }
        public List<TransactionItem> OrderItems { get; set; }
        public string ChildName { get; set; }
        public string ParentName { get; set; }
        public bool ChildHasImage { get; set; }
        public string ChildImage { get; set; }

        private bool _isSelected;
        public bool IsSelected 
        {
            get => _isSelected;
            set => SetProperty(ref _isSelected, value);
        }
    }
}
