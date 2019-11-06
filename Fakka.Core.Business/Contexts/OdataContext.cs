using Fakka.Core.Business.Interfaces;
using Fakka.Core.Business.Models;
using Fakka.Core.Enums;
using Fakka.Core.Managers;
using Fakka.Core.Models;
using Fakka.Core.Services;
using Fakka.Core.Utilities;
using Prism.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fakka.Core.Business.Contexts
{
    public class OdataContext : BaseService, IOdataContext
    {
        public OdataContext(IContainerProvider container) : base(container, "odata")
        {
        }

        public async Task<BaseResponse<Odata<List<ChildProfile>>>> GetChildrenDetails()
        {
            string odataQuery = $"$filter=schoolId%20eq%20{(await SessionManager.GetCurrentSession()).SchoolId}&$orderby=name&$select=id,name,parentId,parentName,schoolId,schoolName,gradeId,gradeName,hasImage,deviceMacAddress," +
                $"currentBalance,dailyLimit,rowVersion,code,gender,hasImage,bannedProducts";

            var response = await DataService.Get<Odata<List<ChildProfile>>>(
                new BaseGetRequest(BaseServiceName, $"ChildSearch?{odataQuery}"), DataSource.Remote, true);
            return response;
        }

        public async Task<BaseResponse<Odata<List<StockItem>>>> GetSchoolStockItems(string schoolId)
        {
            string query = $"$filter=(schoolId%20eq%20{schoolId}%20and%20status%20eq%201)&$orderby=type&$count=true&$select=id,name,type,quantity,unitPrice,schoolId";
            var response = await DataService.Get<Odata<List<StockItem>>>(new BaseGetRequest(BaseServiceName, $"StockItemSearch?{query}"), DataSource.Auto, true);
            return response;
        }

        public async Task<BaseResponse<Odata<List<School>>>> GetSchoolById(string schoolId)
        {
            string query = $"$filter=(id%20eq%20{schoolId})&$select=id,name,educationDepartmentName";
            var response = await DataService.Get<Odata<List<School>>>(new BaseGetRequest(BaseServiceName, $"SchoolSearch?{query}"), DataSource.Remote, true);
            return response;
        }

        public async Task<BaseResponse<Odata<List<OnlineOrder>>>> GetValidOnlineOrderBySchool(string schoolId)
        {
            string query = $"$select=id,name,childId,orderItems,durationFrom,durationTo,hijriDurationFrom,hijriDurationTo,childName,parentName,parentId,childHasImage,childCode";

            var response = await DataService.Get<Odata<List<OnlineOrder>>>(new BaseGetRequest(BaseServiceName, $"TodayOnlineOrders?{query}"), DataSource.Remote, true);
            return response;
        }


    }
}
