using System;
using System.Collections.Generic;
using System.Text;
using Fakka.Core.Business.Enums;
using Prism.Mvvm;

namespace Fakka.Core.Business.Models
{
    public class BaseModels : BindableBase
    {
        public BaseModels()
        {
            // Id = new Guid().ToString();

        }
        public string Id { get; set; }
        public ObjectState State { get; set; }
        public string[] ModifiedProperties { get; set; }
        public byte[] RowVersion { get; set; }
        public bool IsSuccessStatusCode { get; set; }
    }
}
