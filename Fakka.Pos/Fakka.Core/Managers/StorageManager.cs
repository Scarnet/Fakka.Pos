using System;
using System.Net.Http;
using System.Threading.Tasks;
using Fakka.Core.Extensions;
using Fakka.Core.Interfaces;
using Fakka.Core.Models;
using Newtonsoft.Json.Linq;

namespace Fakka.Core.Managers
{
    public class StorageManager : IStorageManager
    {
        private const string AppFolder = "app";
        private const string ApiFolder = "api";
        private const string CustomFolder = "custom";
        private const string LangFolder = "lang";

        private readonly IStorage _storageProvider;
        /*private readonly ISessionManager _sessionManager;*/

        public StorageManager(IStorage storageProvider/*, ISessionManager sessionManager*/)
        {
            _storageProvider = storageProvider;
            /* _sessionManager = sessionManager;*/
        }

        public async Task<T> GetItemAsync<T>(string key, bool isMultiLang = true) where T : BaseOfflineModel, new()
        {
            return await _storageProvider.GetItemAsync<T>(GenerateCustomStorageKey(key, isMultiLang));
        }

        public async Task<T> GetItemAsync<T>(string key) where T : BaseOfflineModel, new()
        {
            return await _storageProvider.GetItemAsync<T>(key);
        }
        
        public async Task<BaseServerResponse<T>> GetItemAsyncByRequest<T>(BaseRequest request)
        {
            //await Task.FromResult(0);
            //return null;

                        var response  =
                            await _storageProvider.GetItemAsync<BaseServerResponse<T>>(GenerateStorageKeyByRequest(request));
            // return Deserialization.JsonStringToObject<T>(res, CaseStrategy.SnakeCase);

            // TODO: Check expiration

            // check In Storage if data exists
            // If found, check expiry
            //JObject obj = JObject.FromObject(result);
            //var response = result;
           // var response = result?.ToObject<BaseServerResponse<T>>();
                        if (response?.Expiration == null || response?.Expiration?.LastUpdated == null) return null;

                        var lastTime = response.Expiration.LastUpdated;
                        var currentTime = new DateTime();
                        // duration difference needs to be recalculated
                        return ((currentTime.Subtract(lastTime) > response.Expiration.ExpirationDuration)) ? null : response;
            // if not expired
            // return data
            // If not found
            // return null and request from server

    
            //await Task.FromResult(0);
            //return null;

        }

        public async Task SetItemAsyncByRequest<T>(BaseRequest request, BaseServerResponse<T> data)
        {
            data.Expiration = data.Expiration ?? new BaseServerExpiration();
            //data.HttpResponse = new HttpResponseMessage();
            //data.Expiration

            //data.Expiration.LastUpdated = new DateTime().Ticks;

            data.OfflineId = GenerateStorageKeyByRequest(request);
            await _storageProvider.SetItemAsync(data);
        }

        public async Task<bool> DeleteItemAsyncByRequest(BaseRequest request)
        {
            return await _storageProvider.DeleteItemAsync<BaseRequest>(GenerateStorageKeyByRequest(request));
        }

        private string GenerateStorageKeyByRequest(BaseRequest request)
        {
            var key =
                $"{ApplicationManager.Instance.GetServerInfo().BaseApiUrl}/{request.ApiController}/{request.ActionMethod.Split('?')[0]}";

            return GenerateStorageKey(ApiFolder, key, true);
        }

        private string GenerateCustomStorageKey(string key, bool isMultiLang)
        {
            return GenerateStorageKey(CustomFolder, key, isMultiLang);
        }

        private string GenerateStorageKey(string folder, string key, bool isMultiLang)
        {
            // TODO: should take into consideration QueryParams
            var lang = isMultiLang ? ApplicationManager.Instance.GetApplicationInfo().Language.ToDescriptionString() : LangFolder;
            var path = $"{AppFolder}/{lang}/{folder}";

            return $"{path}/{key}";
        }

        public Task<bool> SetItemAsync<T>(T data, bool isMultiLang = true) where T : BaseOfflineModel, new()
        {
           return _storageProvider.SetItemAsync(data);
        }

        public Task<bool> DeleteItemAsync<T>(string key, bool isMultiLang = true) where T : BaseOfflineModel
        {
           return _storageProvider.DeleteItemAsync<T>(key);
        }

        public void DeleteAll<T>(bool isMultiLang = true) where T : BaseOfflineModel
        {
            _storageProvider.DeleteAll<T>();
        }

        public async Task<bool> DeleteAll()
        {
            return await _storageProvider.DeleteAll();
        }
    }
}