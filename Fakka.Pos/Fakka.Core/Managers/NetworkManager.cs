using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fakka.Core.Extensions;
using Fakka.Core.Interfaces;
using Fakka.Core.Models;
using Microsoft.IdentityModel.Tokens;

namespace Fakka.Core.Managers
{
    public class NetworkManager
    {
        private readonly INetwork _networkProvider;
        private readonly ISessionManager _sessionManager;
        private readonly int numberOfRetries = 0;
        public NetworkManager(INetwork networkProvider, ISessionManager sessionManager)
        {
            _networkProvider = networkProvider;
            _sessionManager = sessionManager;
        }

        #region public Methods
        public async Task<BaseServerResponse<T>> Get<T>(BaseGetRequest request)
        {
            return await _networkProvider.Get<T>(
                new HttpGetRequest(CreateRequestUrl(request), request.QueryParams, await CreateControlFields(request.Headers)));
        }
        public async Task<BaseServerResponse<T>> Authentication<T>(BaseAuthenticationRequest request)
        {
            return await _networkProvider.Authentication<T>(
                new HttpAuthenticationRequest(CreateRequestUrl(request), request.Body, await CreateControlFields(request.Headers)));
        }
        public async Task<BaseServerResponse<T>> Post<T>(BasePostRequest request)
        {
            return await _networkProvider.Post<T>(
                new HttpPostRequest(CreateRequestUrl(request), request.Body, await CreateControlFields(request.Headers)));
        }

        public async Task<BaseServerResponse<T>> PostFile<T>(BasePostFileRequest request)
        {
            return await _networkProvider.PostFile<T>(
                new HttpPostFileRequest(CreateRequestUrl(request), request.QueryParams, request.File, await CreateControlFields(request.Headers)));
        }
        public async Task<BaseServerResponse<T>> Put<T>(BasePutRequest request)
        {
            return await _networkProvider.Put<T>(
                new HttpPutRequest(CreateRequestUrl(request), request.Body, await CreateControlFields(request.Headers)));
        }
        public async Task<BaseServerResponse<T>> Delete<T>(BaseDeleteRequest request)
        {
            return await _networkProvider.Delete<T>(
                new HttpDeleteRequest(CreateRequestUrl(request), request.Body, await CreateControlFields(request.Headers)));
        }
        public async Task<BaseServerResponse<byte[]>> Download(string url, int retryCounter = 0)
        {
            var response = await _networkProvider.Download(url);

            //Retry downloading for a numberOfReteries time
            if (response.ErrorCode != 0 && retryCounter <= this.numberOfRetries)
                response = await Download(url, ++retryCounter);

            return response;
        }
        #endregion

        #region Private Methods
        private string CreateRequestUrl(BaseRequest request)
        {
            var url =
                $"{ApplicationManager.Instance.GetServerInfo().BaseServerUrl}/{ApplicationManager.Instance.GetServerInfo().BaseApiUrl}";

            if (!string.IsNullOrEmpty(request.ApiController))
                url += $"/{request.ApiController}";

            if (!string.IsNullOrEmpty(request.ActionMethod))
                 url += $"/{ request.ActionMethod}";

            return url;
            // return string.IsNullOrEmpty(request.AbsoluteUrl) ? $"{ApplicationManager.Instance.GetServerInfo().BaseServerUrl}/{ApplicationManager.Instance.GetServerInfo().BaseApiUrl}/{request.ApiController}/{request.ActionMethod}" : request.AbsoluteUrl;
        }


        private async Task<List<KeyValuePair<string, string>>> CreateControlFields(List<KeyValuePair<string, string>> headers)
        {
            headers = (headers?.Count > 0) ? headers : new List<KeyValuePair<string, string>>();

            headers.Add(new KeyValuePair<string, string>("Accept", "application/json"));
            //headers.Add(new KeyValuePair<string, string>("ContentType", "application/json"));

            // CRRN Client Request Refrence Number
            headers.Add(new KeyValuePair<string, string>("Request-Client-Reference-Number", Guid.NewGuid().ToString()));

            if (!await _sessionManager.IsAnonymousSession())
            {
                var currentSession = await _sessionManager.GetCurrentSession();
                headers.Add(
                    new KeyValuePair<string, string>("Authorization", $"Bearer {currentSession.Token}"));
            }


            headers.Add(new KeyValuePair<string, string>("Platform",
            ApplicationManager.Instance.GetTerminalInfo().TerminalCode.ToDescriptionString()));
            headers.Add(new KeyValuePair<string, string>("Version",
                ApplicationManager.Instance.GetApplicationInfo().Version));
            headers.Add(new KeyValuePair<string, string>("Accept-Language",
                ApplicationManager.Instance.GetApplicationInfo().Language.ToDescriptionString()));

            return headers;
        }

        /*private Task<List<KeyValuePair<string, string>>> CreateAuthentictionControlFields(List<KeyValuePair<string, string>> headers)
        {
            headers = (headers?.Count > 0) ? headers : new List<KeyValuePair<string, string>>();

            headers.Add(new KeyValuePair<string, string>("Accept", "application/json"));
            //headers.Add(new KeyValuePair<string, string>("ContentType", "application/json"));

            // CRRN Client Request Refrence Number
            headers.Add(new KeyValuePair<string, string>("Request-Client-Reference-Number", Guid.NewGuid().ToString()));

            //if (!await _sessionManager.IsAnonymousSession())
            //{
            //    var currentSession = await _sessionManager.GetCurrentSession();
            //    headers.Add(
            //        new KeyValuePair<string, string>("Authorization", $"Bearer {currentSession.Token}"));
            //}


            headers.Add(new KeyValuePair<string, string>("Platform",
                ApplicationManager.Instance.GetTerminalInfo().TerminalCode.ToDescriptionString()));
            headers.Add(new KeyValuePair<string, string>("Version",
                ApplicationManager.Instance.GetApplicationInfo().Version));
            headers.Add(new KeyValuePair<string, string>("Accept-Language",
                ApplicationManager.Instance.GetApplicationInfo().Language.ToDescriptionString()));

            return Task.FromResult(headers);
        }*/

        
        #endregion
    }
}