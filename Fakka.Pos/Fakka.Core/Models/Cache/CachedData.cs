using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fakka.Core.Models.Cache
{
    public class CachedData : BaseOfflineModel
    {
        public byte[] Data { get; set; } = new byte[0];
        public DateTime ExpirationDate { get; set; }
    }
}
