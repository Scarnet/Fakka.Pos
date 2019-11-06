using Fakka.Core.Business.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fakka.Core.Business.Models
{
    public class PointOfSaleProfile : BaseModels
    {
        public Guid PointOfSaleId { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string OldEmail { get; set; }
        public string Mobile { get; set; }
        public Status Status { get; set; }
        public Status OldStatus { get; set; }
        public string PointOfSaleName { get; set; }
        public byte[] SchoolImage { get; set; }

    }
}
