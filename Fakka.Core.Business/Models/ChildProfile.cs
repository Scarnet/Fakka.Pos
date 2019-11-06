using Fakka.Core.JsonConverters;
using Fakka.Core.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fakka.Core.Business.Models
{
    public class ChildProfile : BaseModels
    {
        public int Code { get; set; }
        public string ParentId { get; set; }
        public string Name { get; set; }
        public string ParentName { get; set; }
        public string GradeId { get; set; }
        public string GradeName { get; set; }
        public decimal? DailyLimit { get; set; }
        public decimal? CurrentBalance { get; set; }
        public string DeviceMacAddress { get; set; }
        public bool HasImage { get; set; }
        public School School { get; set; }
        [JsonConverter(typeof(JsonStringConverter))]
        public List<BannedProduct> BannedProducts { get; set; }
        public string ChildImage
        {
            get => ImageHandler.GetImageUri(this.Id, $"children/{this.ParentId}");
        }


    }
}
