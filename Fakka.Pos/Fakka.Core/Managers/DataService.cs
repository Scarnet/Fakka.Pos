using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Fakka.Core.Enums;
using Fakka.Core.Exceptions;
using Fakka.Core.Interfaces;
using Fakka.Core.Models;
using Fakka.Core.Models.Cache;
using Newtonsoft.Json.Linq;

namespace Fakka.Core.Managers
{
    public class DataService : IDataService
    {
        private readonly NetworkManager _networkManager;
        private readonly StorageManager _storageManager;

        private readonly ISessionManager _sessionManager;
        // private readonly IView _view;

        public DataService(INetwork networkProvider, IStorage storageProvider, /*IView viewProvider, */ ISessionManager sessionManager)
        {
            _sessionManager = sessionManager;
            // StorageManager storageManager, NetworkManager networkManager, IView viewProvider
            _networkManager = new NetworkManager(networkProvider, _sessionManager);
            _storageManager = new StorageManager(storageProvider/*, sessionManager*/);
            //_view = viewProvider;
        }

        #region public Methods

        public async Task<bool> Invalidate(BaseGetRequest request)
        {
            return await _storageManager.DeleteItemAsyncByRequest(request);
        }

        public async Task<BaseResponse<T>> Get<T>(BaseGetRequest request, DataSource dataSource,
            bool isCacheable,
            List<int> businessErrors)
        {
            switch (dataSource)
            {
                case DataSource.Local: // dataSource is LOCAL
                    var data = await _storageManager.GetItemAsyncByRequest<T>(request);
                    return await ServerResponseHandler(request, data, false, businessErrors);

                case DataSource.Remote: // dataSource is REMOTE
                    return await RemoteGetRequest<T>(request, isCacheable, businessErrors);

                default: // dataSource is Auto ; check in local storage . if not found get from remote
                    // TODO: ASYNC missing implementation
                    var defaultData = await _storageManager.GetItemAsyncByRequest<T>(request);

                    if (defaultData == null)
                        return await RemoteGetRequest<T>(request, isCacheable, businessErrors);
                    else
                        return await ServerResponseHandler(request, defaultData, false, businessErrors);
            }
        }

        public async Task<BaseResponse<T>> Authentication<T>(BaseAuthenticationRequest request, List<int> businessErrors)
        {
            var responseJson = await _networkManager.Authentication<T>(request);
            return await ServerResponseHandler(request, responseJson, false, businessErrors);
        }
        public async Task<BaseResponse<T>> Post<T>(BasePostRequest request, List<int> businessErrors)
        {
            await HandleTokenExpiry();

            var responseJson = await _networkManager.Post<T>(request);
            return await ServerResponseHandler(request, responseJson, false, businessErrors);
        }
        public async Task<BaseResponse<T>> PostFile<T>(BasePostFileRequest request, List<int> businessErrors)
        {
            await HandleTokenExpiry();

            var responseJson = await _networkManager.PostFile<T>(request);
            return await ServerResponseHandler(request, responseJson, false, businessErrors);



        }
        public async Task<BaseResponse<T>> Put<T>(BasePutRequest request, List<int> businessErrors = null)
        {
            await HandleTokenExpiry();

            var responseJson = await _networkManager.Put<T>(request);
            return await ServerResponseHandler(request, responseJson, false, businessErrors);
        }
        public async Task<BaseResponse<T>> Delete<T>(BaseDeleteRequest request, List<int> businessErrors = null)
        {
            await HandleTokenExpiry();

            var responseJson = await _networkManager.Delete<T>(request);
            return await ServerResponseHandler(request, responseJson, false, businessErrors);
        }

        #endregion

        #region private Methods

        private async Task<BaseResponse<T>> RemoteGetRequest<T>(BaseGetRequest request, bool isCacheable,
            List<int> businessErrors)
        {
            await HandleTokenExpiry();
            var responseJson = await _networkManager.Get<T>(request);
            return await ServerResponseHandler(request, responseJson, isCacheable, businessErrors);

        }

        private async Task<BaseResponse<T>> ServerResponseHandler<T>(BaseRequest request,
            BaseServerResponse<T> responseJson, bool isCachable, List<int> businessErrors)
        {

            if (responseJson.HttpResponse.StatusCode == HttpStatusCode.OK)
            {


                if (responseJson.ErrorCode == (int)ErrorCode.NoError)
                {
                    if (isCachable)
                        await _storageManager.SetItemAsyncByRequest(request, responseJson);
                }
                else if (businessErrors != null && businessErrors.Contains(responseJson.ErrorCode))
                {
                    // do nothing (skip error) and proceed as success
                }
                else
                {
                    throw new BusinessException(responseJson.ErrorCode, responseJson.ErrorMessage, responseJson.ErrorDetails);
                }

            }
            else
            {
                return await RequestFailureHandler(responseJson);
            }

            return new BaseResponse<T>(responseJson);
        }

        private async Task<BaseResponse<byte[]>> ServerResponseHandler(BaseServerResponse<byte[]> response, string url)
        {
            if (response.HttpResponse.StatusCode == HttpStatusCode.OK)
            {


                if (response.ErrorCode == (int)ErrorCode.NoError)
                {
                    var cachedData = new CachedData()
                    {
                        Data = response.Data,
                        ExpirationDate = DateTime.Now.AddDays(7),
                        OfflineId = url
                    };
                    await _storageManager.SetItemAsync(cachedData);
                }
                else
                {
                    throw new BusinessException(response.ErrorCode, response.ErrorMessage, response.ErrorDetails);
                }

            }
            else
            {
                //return await RequestFailureHandler(response);
            }

            return new BaseResponse<byte[]>(response);
        }

        private static Task<BaseResponse<T>> RequestFailureHandler<T>(BaseServerResponse<T> responseJson)
        {
            switch (responseJson.HttpResponse.StatusCode)
            {
                case HttpStatusCode.InternalServerError:
                case HttpStatusCode.BadRequest:
                case HttpStatusCode.BadGateway:
                    throw new InternalServerErrorException(responseJson.HttpResponse);

                case HttpStatusCode.NotFound:
                case HttpStatusCode.GatewayTimeout:
                case HttpStatusCode.RequestTimeout:
                case HttpStatusCode.ServiceUnavailable:
                    throw new NetworkException(responseJson.HttpResponse);


                case HttpStatusCode.Unauthorized:
                case HttpStatusCode.Forbidden:
                    throw new AuthorizationException(responseJson.HttpResponse);

                default:
                    throw new BaseException(responseJson.HttpResponse);
            }

            //ex.Message, ex.GetBaseException().Message);
        }

        private async Task HandleTokenExpiry()
        {
            if (await _sessionManager.IsSessionExpired())
            {
                var currentSession = await _sessionManager.GetCurrentSession();
                var response = await Authentication<object>(new BaseAuthenticationRequest("token", null, new
                {
                    Username = currentSession.Username,
                    Password = currentSession.Password,
                    ClientId = ApplicationManager.Instance.GetClientInfo().ClientId,
                    ClientSecret = ApplicationManager.Instance.GetClientInfo().ClientSecret,
                    //Scope = ApplicationManager.Instance.GetClientInfo().Scope,
                    GrantType = ApplicationManager.Instance.GetClientInfo().GrantType,// "refresh_token",
                    //RefreshToken = currentSession .RefreshToken
                }), null);


                var obj = JToken.FromObject(response.Data);
                var token = obj.Value<string>("access_token");
                var refreshToken = obj.Value<string>("refresh_token");
                var expiryDate = DateTime.Now.Add(new TimeSpan(0, 0, 0, obj.Value<int>("expires_in")));

                await _sessionManager.UpdateToken(token, refreshToken, expiryDate);
            }

        }

        public async Task<BaseResponse<byte[]>> Download(string url)
        {
            var response = await _networkManager.Download(url);

            if (response.HttpResponse.StatusCode != HttpStatusCode.OK) ;
               //await RequestFailureHandler(response);

           return await ServerResponseHandler(response, url);
        }

        #endregion
    }
}