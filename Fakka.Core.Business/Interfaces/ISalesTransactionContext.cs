using Fakka.Core.Business.Models;
using Fakka.Core.Business.Models.Dtos;
using Fakka.Core.Interfaces;
using Fakka.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Fakka.Core.Business.Interfaces
{
    public interface ISalesTransactionContext : IBaseDataContext
    {
        Task<BaseResponse<object>> CreateFullTransaction(IEnumerable<TransactionItem> transactionItems, string studentId);
        Task<BaseResponse<object>> CreateFullCashTransaction(IEnumerable<TransactionItem> transactionItems, string studentId);
    }
}
