using Fakka.Core.Business.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fakka.Core.Business.Models.Dtos
{
    public class TransactionDto : BaseTransactionDto
    {
        public TransactionDto()
        {
            TransactionItems = new List<TransactionItem>();
        }
        public IList<TransactionItem> TransactionItems { get; set; }

    }
}
