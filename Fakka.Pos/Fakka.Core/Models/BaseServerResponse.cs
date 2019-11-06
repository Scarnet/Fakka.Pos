using SQLite;
using System;
using System.Net;
using System.Net.Http;

namespace Fakka.Core.Models
{
    /// <summary>
    /// Object returned by the server
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BaseServerResponse<T> : BaseOfflineModel
    {
       /* public BaseServerResponse()
        {
            Version
            System.Version
            None.

                Content
            System.Net.Http.HttpContent
            None.

                StatusCode
            System.Net.HttpStatusCode
            None.

                ReasonPhrase
            string
            None.

                Headers
                Collection of Object
            None.

                RequestMessage
            System.Net.Http.HttpRequestMessage
            None.

                IsSuccessStatusCode
        }
                */

        public HttpResponse HttpResponse { get; set; }
        // public string AccessToken { get; set; }
        public int ErrorCode { get; set; }
        public int Error { get; set; }
        public string ErrorDetails { get; set; }
        public string ErrorMessage { get; set; }
        public string ErrorDescription { get; set; }
        public T Data { get; set; }
        public BaseServerExpiration Expiration { get; set; }
    }

    public class BaseServerExpiration
    {
        public BaseServerExpiration()
        {
            this.LastUpdated = DateTime.Now;
            this.ExpirationDuration = new TimeSpan(0, 0, 2, 0);
        }
        public TimeSpan ExpirationDuration { get; set; }
        public DateTime LastUpdated { get; set; }
    }

    public class BaseServerResponse : BaseServerResponse<object>
    {
    }

    [Table("HttpResponse")]
    public class HttpResponse
    {
        public HttpStatusCode StatusCode { get; set; }
        public string ReasonPhrase { get; set; }

    }
}