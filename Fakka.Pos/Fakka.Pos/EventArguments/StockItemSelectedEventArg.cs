using Fakka.Core.Business.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fakka.Pos.EventArguments
{
    public class StockItemSelectedEventArg : EventArgs
    {
        public bool Selected { get; set; }
        public StockItem StockItem { get; set; }
    }
}
