using Fakka.Core.Business.Interfaces;
using Fakka.Core.Business.Models;
using Fakka.Core.Enums;
using Fakka.Core.Managers;
using Fakka.Core.Models;
using Fakka.Core.Services;
using Prism.Ioc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Fakka.Core.Business.Contexts
{
    public class GradeContext : BaseService, IGradeContext
    {
        public GradeContext(IContainerProvider container) : base(container, "Grade")
        {
        }

        public async Task<BaseResponse<List<Grade>>> GetGradesBySchool(string schoolId)
        {
            var response = await DataService.Get<List<Grade>>(
                new BaseGetRequest(BaseServiceName, $"getGradesBySchool/{schoolId}"),
                DataSource.Auto, false);
            return response;
        }
    }
}
