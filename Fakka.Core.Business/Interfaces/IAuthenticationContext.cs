using System.Threading.Tasks;
using Fakka.Core.Business.Models;
using Fakka.Core.Interfaces;
using Fakka.Core.Models;

namespace Fakka.Core.Business.Interfaces
{
    public interface IAuthenticationContext : IBaseDataContext
    {
        Task<BaseResponse<AuthenticationResponse>> Login(string username, string password);
    }
}
