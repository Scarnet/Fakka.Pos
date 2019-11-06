using Fakka.Core.Business.Models;
using Fakka.Core.Interfaces;
using Fakka.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Fakka.Core.Business.Interfaces
{
    public interface IGradeContext : IBaseDataContext
    {
        Task<BaseResponse<List<Grade>>> GetGradesBySchool(string schoolId);

    }
}
