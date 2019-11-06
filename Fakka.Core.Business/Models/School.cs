using Fakka.Core.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fakka.Core.Business.Models
{
    public class School : BaseModels
    {
        public string Name { get; set; }
        public string EducationDepartmentId { get; set; }
        public string Type { get; set; }
        [JsonProperty("educationDepartmentName")]
        public string EducationalDepartmentName { get; set; }

        public string Image
        {
            get => ImageHandler.GetImageUri(this.Id, "schools");
        }

    }
}
