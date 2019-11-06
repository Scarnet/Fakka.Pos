using System.Collections.Generic;
using System.Threading.Tasks;
using Fakka.Core.Enums;
using Fakka.Core.Models;

namespace Fakka.Core.Interfaces
{
    public interface IDataService
    {
        Task<BaseResponse<T>> Get<T>(BaseGetRequest request, DataSource dataSource,
            bool isCacheable, List<int> businessErrors = null);

        Task<bool> Invalidate(BaseGetRequest request);

        Task<BaseResponse<T>> Authentication<T>(BaseAuthenticationRequest request, List<int> businessErrors = null);
        Task<BaseResponse<T>> Post<T>(BasePostRequest request, List<int> businessErrors = null);
        Task<BaseResponse<T>> PostFile<T>(BasePostFileRequest request, List<int> businessErrors = null);
        Task<BaseResponse<T>> Put<T>(BasePutRequest request, List<int> businessErrors = null);
        Task<BaseResponse<T>> Delete<T>(BaseDeleteRequest request, List<int> businessErrors = null);
        Task<BaseResponse<byte[]>> Download(string url);
    }
}