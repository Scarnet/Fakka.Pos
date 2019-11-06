using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Fakka.Core.Enums;
using Fakka.Core.Interfaces;
using Fakka.Core.Models;
using Fakka.Core.Utilities;

namespace Fakka.Core.Providers
{
    public class HttpProvider : INetwork
    {
        #region Properties
        private const int DefaultPoolSize = 10;
        private readonly Queue<HttpClient> _httpClientQueue;
        private readonly HttpMessageHandler _httpFilter;
        private readonly SemaphoreSlim _semaphore;
        #endregion

        #region Constructor
        public HttpProvider()
        {
            _httpFilter = GetDefaultFilter();
            _semaphore = new SemaphoreSlim(DefaultPoolSize);
            _httpClientQueue = new Queue<HttpClient>();
        }
        #endregion

        #region public Methods
        public async Task<BaseServerResponse<T>> Get<T>(HttpGetRequest request)
        {
            HttpClient client = null;

            try
            {
                client = await GetHttpClientInstance();
                //Add Control Fields To Request Headers
                request.Headers.ForEach(kv =>
                {
                    client.DefaultRequestHeaders.Remove(kv.Key); // keep headers updated
                    client.DefaultRequestHeaders.Add(kv.Key, kv.Value);
                });


                var httpRequestMessage = new HttpRequestMessage(request.Method, $"{request.Url}" + (string.IsNullOrEmpty(request.QueryString) ? "" : $"?{request.QueryString}"));

                var httpResponseMessage = await client.SendAsync(httpRequestMessage);

                FixInvalidCharset(httpResponseMessage);

                var result = await ResolveHttpResponse<T>(httpResponseMessage, null);

                return result;
            }
            finally
            {
                // Add the HttpClient instance back to the queue.
                if (client != null)
                    _httpClientQueue.Enqueue(client);
            }
        }
        public async Task<BaseServerResponse<T>> Authentication<T>(HttpAuthenticationRequest request)
        {
            HttpClient client = null;

            try
            {
                client = await GetHttpClientInstance();
                //Add Control Fields To Request Headers
                request.Headers.ForEach(kv =>
                {
                    client.DefaultRequestHeaders.Remove(kv.Key); // keep headers updated
                    client.DefaultRequestHeaders.Add(kv.Key, kv.Value);
                });

                var httpRequestMessage = new HttpRequestMessage(request.Method, request.Url)
                {

                    Content = new StringContent(request.Body)
                };
                httpRequestMessage.Content.Headers.ContentType = new MediaTypeHeaderValue(request.ContentType);

                var httpResponseMessage = await client.SendAsync(httpRequestMessage);

                FixInvalidCharset(httpResponseMessage);

                var result = await ResolveHttpResponse<T>(httpResponseMessage, CaseStrategy.SnakeCase);
                return result;
            }
            finally
            {
                // Add the HttpClient instance back to the queue.
                if (client != null)
                    _httpClientQueue.Enqueue(client);
            }
        }
        public async Task<BaseServerResponse<T>> Post<T>(HttpPostRequest request)
        {
            HttpClient client = null;

            try
            {
                client = await GetHttpClientInstance();
                //Add Control Fields To Request Headers
                request.Headers.ForEach(kv =>
                {
                    client.DefaultRequestHeaders.Remove(kv.Key); // keep headers updated
                    client.DefaultRequestHeaders.Add(kv.Key, kv.Value);
                });

                var httpRequestMessage = new HttpRequestMessage(request.Method, request.Url)
                {

                    Content = new StringContent(request.Body)
                };
                httpRequestMessage.Content.Headers.ContentType = new MediaTypeHeaderValue(request.ContentType);

                var httpResponseMessage = await client.SendAsync(httpRequestMessage);

                FixInvalidCharset(httpResponseMessage);

                var result = await ResolveHttpResponse<T>(httpResponseMessage, null);
                return result;
            }
            finally
            {
                // Add the HttpClient instance back to the queue.
                if (client != null)
                    _httpClientQueue.Enqueue(client);
            }
        }
        public async Task<BaseServerResponse<T>> PostFile<T>(HttpPostFileRequest request)
        {
            HttpClient client = null;

            try
            {
                client = await GetHttpClientInstance();
                //Add Control Fields To Request Headers
                request.Headers.ForEach(kv =>
                {
                    client.DefaultRequestHeaders.Remove(kv.Key); // keep headers updated
                    client.DefaultRequestHeaders.Add(kv.Key, kv.Value);
                });

                var multipartForm = new MultipartFormDataContent();
                multipartForm.Add(request.File.Content, request.File.Name, request.File.FilePath);


                var httpRequestMessage = new HttpRequestMessage(request.Method, $"{request.Url}?{request.QueryString}")
                {

                    Content = multipartForm

                };


                // httpRequestMessage.Content.Headers.ContentType = new MediaTypeHeaderValue(request.ContentType);
                var httpResponseMessage = await client.SendAsync(httpRequestMessage);

                FixInvalidCharset(httpResponseMessage);

                var result = await ResolveHttpResponse<T>(httpResponseMessage, null);

                return result;
            }
            finally
            {
                // Add the HttpClient instance back to the queue.
                if (client != null)
                    _httpClientQueue.Enqueue(client);
            }
        }
        public async Task<BaseServerResponse<T>> Put<T>(HttpPutRequest request)
        {
            HttpClient client = null;

            try
            {
                client = await GetHttpClientInstance();
                //Add Control Fields To Request Headers
                request.Headers.ForEach(kv =>
                {
                    client.DefaultRequestHeaders.Remove(kv.Key); // keep headers updated
                    client.DefaultRequestHeaders.Add(kv.Key, kv.Value);
                });

                var httpRequestMessage = new HttpRequestMessage(request.Method, request.Url)
                {

                    Content = new StringContent(request.Body)
                };
                httpRequestMessage.Content.Headers.ContentType = new MediaTypeHeaderValue(request.ContentType);

                var httpResponseMessage = await client.SendAsync(httpRequestMessage);

                FixInvalidCharset(httpResponseMessage);

                var result = await ResolveHttpResponse<T>(httpResponseMessage, null);
                return result;
            }
            finally
            {
                // Add the HttpClient instance back to the queue.
                if (client != null)
                    _httpClientQueue.Enqueue(client);
            }
        }
        public async Task<BaseServerResponse<T>> Delete<T>(HttpDeleteRequest request)
        {
            HttpClient client = null;

            try
            {
                client = await GetHttpClientInstance();
                //Add Control Fields To Request Headers
                request.Headers.ForEach(kv =>
                {
                    client.DefaultRequestHeaders.Remove(kv.Key); // keep headers updated
                    client.DefaultRequestHeaders.Add(kv.Key, kv.Value);
                });

                var httpRequestMessage = new HttpRequestMessage(request.Method, request.Url)
                {

                    Content = new StringContent(request.Body)
                };
                httpRequestMessage.Content.Headers.ContentType = new MediaTypeHeaderValue(request.ContentType);

                var httpResponseMessage = await client.SendAsync(httpRequestMessage);

                FixInvalidCharset(httpResponseMessage);

                var result = await ResolveHttpResponse<T>(httpResponseMessage, null);
                return result;
            }
            finally
            {
                // Add the HttpClient instance back to the queue.
                if (client != null)
                    _httpClientQueue.Enqueue(client);
            }
        }

        #endregion

        #region private Methods
        private static HttpMessageHandler GetDefaultFilter()
        {
            return new HttpClientHandler();
        }

        private async Task<HttpClient> GetHttpClientInstance()
        {
            await _semaphore.WaitAsync().ConfigureAwait(false);

            var client = new HttpClient(_httpFilter)
            {
                Timeout = TimeSpan.FromMilliseconds(60000)
            };

            // Try and get HttpClient from the queue
            if (_httpClientQueue.Any())
            {
                var httpClient = _httpClientQueue.Dequeue();
                if (httpClient != null)
                    client = httpClient;
            }

            client.DefaultRequestHeaders.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json"));
            client.DefaultRequestHeaders.UserAgent.Add(
                new ProductInfoHeaderValue(new ProductHeaderValue("Xamarin_HttpClient", "1.0")));

            _semaphore.Release();
            return client;
        }

        /// <summary>
        ///     Fix invalid charset returned by some web sites.
        /// </summary>
        /// <param name="response">HttpResponseMessage instance.</param>
        private void FixInvalidCharset(HttpResponseMessage response)
        {
            if (response?.Content?.Headers?.ContentType?.CharSet != null)
            {
                // Fix invalid charset returned by some web sites.
                var charset = response.Content.Headers.ContentType.CharSet;
                if (charset.Contains("\""))
                    response.Content.Headers.ContentType.CharSet = charset.Replace("\"", string.Empty);
            }
        }

        private async Task<BaseServerResponse<T>> ResolveHttpResponse<T>(HttpResponseMessage httpResponseMessage, CaseStrategy? caseStrategy)
        {
            var response = await httpResponseMessage.Content.ReadAsStringAsync();
            BaseServerResponse<T> result = null;
            if (IsValidHttpResponse(httpResponseMessage.StatusCode))
            {
                httpResponseMessage.StatusCode = HttpStatusCode.OK;
                result = new BaseServerResponse<T>()
                {
                    Data = Deserialization.JsonStringToObject<T>(response, caseStrategy)
                };
            }
            else
            {
                result = Deserialization.JsonStringToObject<BaseServerResponse<T>>(response, caseStrategy);
                if (result.ErrorCode != (int)ErrorCode.NoError)
                {
                    httpResponseMessage.StatusCode = HttpStatusCode.OK;
                }
                // to Handler Authentication response that is different from every other response
                if (result.Error != (int)ErrorCode.NoError)
                {
                    httpResponseMessage.StatusCode = HttpStatusCode.OK;
                    result.ErrorCode = result.Error;
                    result.ErrorMessage = result.ErrorDescription;
                }
            }

            result.HttpResponse = new HttpResponse { StatusCode = httpResponseMessage.StatusCode, ReasonPhrase = httpResponseMessage.ReasonPhrase };
            return result;
        }

        //Handle creating response object for bytes content
        private async Task<BaseServerResponse<byte[]>> ResolveHttpResponse(HttpResponseMessage httpResponseMessage)
        {
            var response = await httpResponseMessage.Content.ReadAsByteArrayAsync();
            BaseServerResponse<byte[]> result = null;
            if (IsValidHttpResponse(httpResponseMessage.StatusCode))
            {
                httpResponseMessage.StatusCode = HttpStatusCode.OK;
                result = new BaseServerResponse<byte[]>()
                {
                    Data = response
                };
            }
            else
            {
                result = new BaseServerResponse<byte[]>()
                {
                    Data = null,
                    ErrorCode = (int)ErrorCode.BaseError
                };

                if (result.ErrorCode != (int)ErrorCode.NoError)
                {
                    httpResponseMessage.StatusCode = HttpStatusCode.OK;
                }
                // to Handler Authentication response that is different from every other response
                if (result.Error != (int)ErrorCode.NoError)
                {
                    httpResponseMessage.StatusCode = HttpStatusCode.OK;
                    result.ErrorCode = result.Error;
                    result.ErrorMessage = result.ErrorDescription;
                }

            }

            result.HttpResponse = new HttpResponse { StatusCode = httpResponseMessage.StatusCode, ReasonPhrase = httpResponseMessage.ReasonPhrase };
            return result;
        }

        private bool IsValidHttpResponse(HttpStatusCode statusCode)
        {
            return statusCode == HttpStatusCode.OK;
        }

        public async Task<BaseServerResponse<byte[]>> Download(string url)
        {
            var client = await GetHttpClientInstance();
            var response = await client.GetAsync(url);

            var result = await ResolveHttpResponse(response);
            return result;
        }

        #endregion
    }
}