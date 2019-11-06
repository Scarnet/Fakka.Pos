using System.Collections.Generic;
using System.Net.Http;
using Fakka.Core.Enums;
using Fakka.Core.Extensions;
using Fakka.Core.Utilities;

namespace Fakka.Core.Models
{
    public class HttpRequest
    {
        protected HttpRequest(string url, List<KeyValuePair<string, string>> headers)
        {
            Headers = headers;
            Url = url;
        }

        public HttpMethod Method { get; set; }
        public string ContentType { get; set; }
        public string Url { get; set; }
        public List<KeyValuePair<string, string>> Headers { get; set; }
    }

    public class HttpGetRequest : HttpRequest
    {
        public HttpGetRequest(string url, object queryParameters, List<KeyValuePair<string, string>> headers)
            : base(url, headers)
        {
            Method = HttpMethod.Get;
            QueryString = Serialization.ObjectToKeyValueString(queryParameters);

        }

        public string QueryString { get; set; }
    }

    public class HttpAuthenticationRequest : HttpRequest
    {
        public HttpAuthenticationRequest(string url, object body, List<KeyValuePair<string, string>> headers)
            : base(url, headers)
        {
            Method = HttpMethod.Post;


            ContentType = Enums.ContentType.Form.ToDescriptionString();
            // if form encoded ONLY USED FOR MS Identity Authentication (requires SnakeCase)
            // query string format in GET is similar to Forms variables in POST
            Body = Serialization.ObjectToKeyValueString(body, CaseStrategy.SnakeCase);

        }

        public string Body { get; set; }

    }

    public class HttpPostRequest : HttpRequest
    {
        public HttpPostRequest(string url, object body, List<KeyValuePair<string, string>> headers)
            : base(url, headers)
        {
            Method = HttpMethod.Post;


            ContentType = Enums.ContentType.Json.ToDescriptionString();
            Body = Serialization.ObjectToJsonString(body);
        }

        public string Body { get; set; }

    }

    public class HttpPostFileRequest : HttpRequest
    {
        public HttpPostFileRequest(string url, object queryParameters, File file, List<KeyValuePair<string, string>> headers)
            : base(url, headers)
        {
            Method = HttpMethod.Post;
            ContentType = Enums.ContentType.Multipart.ToDescriptionString();
            QueryString = Serialization.ObjectToKeyValueString(queryParameters);

            //FileName = file.Name;
            //FilePath = file.FilePath;
            //FileStream = file.Content;

            File = file;

        }

        //public string FilePath { get; set; }
        //public string FileName { get; set; }
        //public StreamContent FileStream { get; set; }
        public File File { get; set; }

        public string QueryString { get; set; }

    }

    public class HttpPutRequest : HttpRequest
    {
        public HttpPutRequest(string url, object body, List<KeyValuePair<string, string>> headers)
            : base(url, headers)
        {
            Method = HttpMethod.Put;
            ContentType = Enums.ContentType.Json.ToDescriptionString();
            Body = Serialization.ObjectToJsonString(body);


        }

        public string Body { get; set; }

    }
    public class HttpDeleteRequest : HttpRequest
    {
        public HttpDeleteRequest(string url, object body, List<KeyValuePair<string, string>> headers)
            : base(url, headers)
        {
            Method = HttpMethod.Delete;
            ContentType = Enums.ContentType.Json.ToDescriptionString();
            Body = Serialization.ObjectToJsonString(body);

        }

        public string Body { get; set; }

    }
}