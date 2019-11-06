using Fakka.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fakka.Core.Business.Models
{
    public class BannedProduct : BaseModels
    {
        public string Name { get; set; }

        public string Image
        {
            get { return ImageHandler.GetImageUri(this.Id.ToLower(), "products"); }
        }

    }
}
