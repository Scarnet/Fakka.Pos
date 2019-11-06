using System;
using System.Globalization;
using System.Threading.Tasks;
using Fakka.Core.Interfaces;
using Fakka.Core.Models;

namespace Fakka.Core.Managers
{
    public class SessionManager : ISessionManager
    {
        #region Session 
        private readonly IStorageManager _storageManager;

        public SessionManager(IStorageManager storageManager)
        {
            _storageManager = storageManager;
        }

        public async Task StartSession(UserSession userSession)
        {
            userSession.OfflineId = nameof(userSession);
            await _storageManager.SetItemAsync(userSession);
        }
        public async void EndSession()
        {
            await _storageManager.DeleteItemAsync<UserSession>(nameof(UserSession), false);

        }
        public async Task<UserSession> GetCurrentSession()
        {
            return await _storageManager.GetItemAsync<UserSession>(nameof(UserSession), false);
        }
        public async Task<bool> IsAnonymousSession()
        {
            var userSession = await GetCurrentSession();

            return string.IsNullOrEmpty(userSession?.Token)
                || string.IsNullOrEmpty(userSession?.Username)
                || string.IsNullOrEmpty(userSession?.Password);

        }
        public async Task<bool> IsSessionExpired()
        {
            var userSession = await GetCurrentSession();
            if (userSession == null) return false; // there is no session in the first place, so it has not expired

            // TODO: check expiry too
            return DateTime.Now.Ticks > userSession?.ExpiryDate.Ticks;
        }
        public async Task UpdateToken(string accessToken, string refreshToken, DateTime expiryDate)
        {
            var userSession = await GetCurrentSession();
            userSession.UpdateToken(accessToken, refreshToken, expiryDate);
            userSession.OfflineId = nameof(UserSession);
            await _storageManager.SetItemAsync(userSession, false);
        }

        public async Task UpdatePosProfile(string pointOfSaleId)
        {
            var userSession = await GetCurrentSession();

            userSession.PointOfSaleId = pointOfSaleId;
            await _storageManager.SetItemAsync(userSession, false);
        }

        #endregion

    }
}