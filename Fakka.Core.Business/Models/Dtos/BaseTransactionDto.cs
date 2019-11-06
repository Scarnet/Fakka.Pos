using Fakka.Core.Business.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fakka.Core.Business.Models.Dtos
{
    public class BaseTransactionDto : BaseModels
    {
        public Guid? StudentId { get; set; }
        public Guid PointOfSaleId { get; set; }
        public PointOfSaleTransactionType Type { get; set; }
        public PointOfSaleTransactionFinancialType TransactionFinancialType { get; set; }
        public decimal Total { get; set; }

    }
}
