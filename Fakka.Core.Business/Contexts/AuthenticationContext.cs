using System;
using System.Linq;
using System.Threading.Tasks;
using Fakka.Core.Business.Interfaces;
using Fakka.Core.Business.Models;
using Fakka.Core.Business.Resources;
using Fakka.Core.Enums;
using Fakka.Core.Exceptions;
using Fakka.Core.Extensions;
using Fakka.Core.Managers;
using Fakka.Core.Models;
using Fakka.Core.Services;
using Prism.Ioc;
using Xamarin.Forms;

namespace Fakka.Core.Business.Contexts
{
    public class AuthenticationContext : BaseService, IAuthenticationContext
    {
        public AuthenticationContext(IContainerProvider container) : base(container, "")
        {
        }

        public async Task<BaseResponse<AuthenticationResponse>> Login(string username, string password)
        {
            StateManager.Instance.DeleteAll();
            // System.Diagnostics.Debug.WriteLine("Login Started" + DateTime.Now.ToString());
            var response = await DataService.Authentication<AuthenticationResponse>(
                new BaseAuthenticationRequest("token", null, new
                {
                    Username = username,
                    Password = password,
                    ClientId = ApplicationManager.Instance.GetClientInfo().ClientId,
                    ClientSecret = ApplicationManager.Instance.GetClientInfo().ClientSecret,
                    GrantType = ApplicationManager.Instance.GetClientInfo().GrantType,
                    Role = UserRole.PointOfSaleUser
                    //Scope = Settings.AuthenticationSettings.Scope,
                }));

            // System.Diagnostics.Debug.WriteLine("Login Ended" + DateTime.Now.ToString());
            // return response;
            if (response.SuccessCode != (int)ErrorCode.NoError
            || response.Data == null)
                return response;

            //// TODO: Decode JWT is not working
            // var decodedToken = Jwt.DecodeBearerToken(response.Data.AccessToken);

            var userRole = response.Data.Role.GetValueFromDescription<UserRole>();
            //ValidateUserRole(userRole);

            await SessionManager.StartSession(
                new UserSession(username, password, response.Data.AccessToken, response.Data.RefreshToken,
                    (int)userRole, response.Data.Id, DateTime.Now.Add(new TimeSpan(0, 0, 0, response.Data.ExpiresIn)), response.Data.ReferenceId));

            return response;
        }

        private void ValidateUserRole(UserRole role)
        {
            bool validRole = ApplicationManager.Instance.GetClientInfo().ValidRoles.Contains(role);
            if (!validRole)
                throw new BusinessException((int)ErrorCode.BaseError, BusinessResources.InvalidUserLogin, BusinessResources.InvalidUserLogin);
        }
    }
}
