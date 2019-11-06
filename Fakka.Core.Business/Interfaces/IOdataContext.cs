using Fakka.Core.Business.Models;
using Fakka.Core.Interfaces;
using Fakka.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Fakka.Core.Business.Interfaces
{
    public interface IOdataContext : IBaseDataContext
    {
        Task<BaseResponse<Odata<List<StockItem>>>> GetSchoolStockItems(string schoolId);
        Task<BaseResponse<Odata<List<ChildProfile>>>> GetChildrenDetails();
        Task<BaseResponse<Odata<List<School>>>> GetSchoolById(string schoolId);
        Task<BaseResponse<Odata<List<OnlineOrder>>>> GetValidOnlineOrderBySchool(string schoolId);

    }
}
