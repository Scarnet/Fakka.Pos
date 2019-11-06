using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fakka.Core.Models
{
    public class BaseOfflineModel 
    {
        [PrimaryKey]
        public string OfflineId { get; set; }
    }
}
