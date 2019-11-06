using Fakka.Core.Business.Enums;
using Fakka.Core.Business.Interfaces;
using Fakka.Core.Business.Models;
using Fakka.Core.Business.Models.Dtos;
using Fakka.Core.Interfaces;
using Fakka.Core.Models;
using Fakka.Core.Services;
using Prism.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fakka.Core.Business.Contexts
{
    public class SalesTransactionContext : BaseService, ISalesTransactionContext
    {
        public SalesTransactionContext(IContainerProvider container) : base(container, "SaleTransaction")
        {
        }

        public async Task<BaseResponse<object>> CreateFullCashTransaction(IEnumerable<TransactionItem> transactionItems, string studentId)
        {
            var currentSession = await SessionManager.GetCurrentSession();
            string posId = currentSession.PointOfSaleId;
            decimal totalOrder = transactionItems.Sum(item => item.UnitPrice * item.Quantity);

            var transaction = new TransactionDto()
            {
                StudentId = studentId != null ? (Guid?)new Guid(studentId) : null,
                PointOfSaleId = new Guid(posId),
                Total = totalOrder,
                TransactionFinancialType = PointOfSaleTransactionFinancialType.PrePaid,
                TransactionItems = transactionItems.ToList(),
                Type = PointOfSaleTransactionType.Full,
            };

            var response = await DataService.Post<object>(new BasePostRequest(BaseServiceName, "createFullCashTransaction", transaction));

            return response;
        }

        public async Task<BaseResponse<object>> CreateFullTransaction(IEnumerable<TransactionItem> transactionItems, string studentId)
        {
            var currentSession = await SessionManager.GetCurrentSession();
            string posId = currentSession.PointOfSaleId;
            decimal totalOrder = transactionItems.Sum(item => item.UnitPrice * item.Quantity);

            var transaction = new TransactionDto()
            {
                StudentId = new Guid(studentId),
                PointOfSaleId = new Guid(posId),
                Total = totalOrder,
                TransactionFinancialType = PointOfSaleTransactionFinancialType.PrePaid,
                TransactionItems = transactionItems.ToList(),
                Type = PointOfSaleTransactionType.Full,
            };

            var response = await DataService.Post<object>(new BasePostRequest(BaseServiceName, "createFullTransaction", transaction));

            return response;
        }
    }
}
