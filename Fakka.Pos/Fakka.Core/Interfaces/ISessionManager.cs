using System;
using System.Threading.Tasks;
using Fakka.Core.Models;

namespace Fakka.Core.Interfaces
{
    public interface ISessionManager
    {
        Task StartSession(UserSession userSession);
        void EndSession();
        Task<UserSession> GetCurrentSession();
        Task<bool> IsAnonymousSession();
        Task<bool> IsSessionExpired();
        Task UpdateToken(string accessToken, string refreshToken, DateTime expiryDate);
        Task UpdatePosProfile(string pointOfSaleId);

    }
}
