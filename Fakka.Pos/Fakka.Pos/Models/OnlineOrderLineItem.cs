using System;
using System.Collections.Generic;
using System.Text;

namespace Fakka.Pos.Models
{
    public class OnlineOrderLineItem : LineItem
    {
        public bool IsOnlineOrder => true;
    }
}
