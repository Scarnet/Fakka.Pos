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
    public class SchoolContext : BaseService, ISchoolContext 
    {
        public SchoolContext(IContainerProvider container) : base(container, "School")
        {
        }

        public async Task<BaseResponse<School>> GetSchool(string schoolId)
        {
            var response = await DataService.Get<School>(new BaseGetRequest(BaseServiceName, $"get/{schoolId}/"), DataSource.Auto, true);
            return response;
        }
    }
}
