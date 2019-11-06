using Fakka.Core.Business.Interfaces;
using Fakka.Core.Business.Models;
using Fakka.Core.Enums;
using Fakka.Core.Models;
using Fakka.Core.Services;
using Prism.Ioc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Fakka.Core.Business.Contexts
{
    public class PosUserContext : BaseService, IPosUserContext
    {
        public PosUserContext(IContainerProvider container) : base(container, "PosUser")
        {
        }

        public async Task<BaseResponse<PointOfSaleProfile>> GetProfile(string userId)
        {
            var response = await DataService.Get<PointOfSaleProfile>(new BaseGetRequest(BaseServiceName, $"getWithDetails/{userId}"), DataSource.Auto, true);
            await SessionManager.UpdatePosProfile(response.Data.PointOfSaleId.ToString());
            return response;
        }
    }
}
