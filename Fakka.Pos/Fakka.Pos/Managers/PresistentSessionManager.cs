using Fakka.Core.Interfaces;
using Fakka.Core.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Fakka.Pos.Managers
{
    public class PresistentSessionManager : ISessionManager
    {
        public void EndSession()
        {
            if (!HasStoredSession())
                return;

            App.Current.Properties.Remove(nameof(UserSession));
            App.Current.SavePropertiesAsync();
        }

        public async Task<UserSession> GetCurrentSession()
        {
            if (!HasStoredSession())
                return null;

            string jsonSession = App.Current.Properties[nameof(UserSession)].ToString();
            var session = JsonConvert.DeserializeObject<UserSession>(jsonSession);
            return session;
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

        public async Task StartSession(UserSession userSession)
        {
            string jsonSession = JsonConvert.SerializeObject(userSession);

            if (HasStoredSession())
                App.Current.Properties[nameof(UserSession)] = jsonSession;
            else
                App.Current.Properties.Add(nameof(UserSession), jsonSession);
            
            await App.Current.SavePropertiesAsync();
        }

        public async Task UpdatePosProfile(string pointOfSaleId)
        {
            var userSession = await GetCurrentSession();
            userSession.PointOfSaleId = pointOfSaleId;
            await StartSession(userSession);
        }

        public async Task UpdateToken(string accessToken, string refreshToken, DateTime expiryDate)
        {
            var userSession = await GetCurrentSession();
            userSession.UpdateToken(accessToken, refreshToken, expiryDate);
            await StartSession(userSession);
        }


        private bool HasStoredSession()
        {
            return App.Current.Properties.ContainsKey(nameof(UserSession));
        }
    }
}
