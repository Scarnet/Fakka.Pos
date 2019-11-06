using System.Threading.Tasks;
using Fakka.Core.Models;

namespace Fakka.Core.Interfaces
{
    public interface INetwork
    {
        Task<BaseServerResponse<T>> Get<T>(HttpGetRequest request);
        Task<BaseServerResponse<T>> Authentication<T>(HttpAuthenticationRequest request);
        Task<BaseServerResponse<T>> Post<T>(HttpPostRequest request);
        Task<BaseServerResponse<T>> PostFile<T>(HttpPostFileRequest request);
        Task<BaseServerResponse<T>> Put<T>(HttpPutRequest request);
        Task<BaseServerResponse<T>> Delete<T>(HttpDeleteRequest request);
        Task<BaseServerResponse<byte[]>> Download(string url);

    }
}