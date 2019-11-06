using System.Collections.Generic;

namespace Fakka.Core.Models
{
    public class BaseRequest : BaseOfflineModel
    {
        /// <summary>
        /// Object used to create api requests with custom params for GET/POST
        /// </summary>
        /// <param name="apiController"></param>
        /// <param name="actionMethod"></param>
        /// <param name="headers"></param>
        protected BaseRequest(string apiController, string actionMethod, List<KeyValuePair<string, string>> headers = null)
        {
            ApiController = apiController;
            ActionMethod = actionMethod;
            Headers = headers;

        }

        public string ApiController { get; } // controller
        public string ActionMethod { get; } // action
        public List<KeyValuePair<string, string>> Headers { get; } // headers
    }
    public class BaseGetRequest : BaseRequest
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="apiController"></param>
        /// <param name="actionMethod"></param>
        /// <param name="queryParams">ex: new { queryParamKey = value, Id = 0 }</param>
        /// <param name="headers"></param>
        public BaseGetRequest(string apiController, string actionMethod, object queryParams = null, List<KeyValuePair<string, string>> headers = null)
            : base(apiController, actionMethod, headers)
        {
            QueryParams = queryParams;
        }

        public object QueryParams { get; }
    }
    public class BasePostRequest : BaseRequest
    {
        /// <summary>
        /// </summary>
        /// <param name="apiController"></param>
        /// <param name="actionMethod"></param>
        /// <param name="body"> ex: new { propertyKey = value, Id = 0 }</param>
        /// <param name="headers"></param>
        public BasePostRequest(string apiController, string actionMethod, object body, List<KeyValuePair<string, string>> headers = null)
            : base(apiController, actionMethod, headers)
        {
            Body = body;
        }

        public object Body { get; }


    }
    public class BaseAuthenticationRequest : BaseRequest
    {
       /// <summary>
        /// </summary>
        /// <param name="apiController"></param>
        /// <param name="actionMethod"></param>
        /// <param name="body"> ex: new { propertyKey = value, Id = 0 }</param>
        /// <param name="headers"></param>
        public BaseAuthenticationRequest(string apiController, string actionMethod, object body, List<KeyValuePair<string, string>> headers = null)
            : base(apiController, actionMethod, headers)
        {
            Body = body;
        }

        public object Body { get; }


    }
    public class BasePostFileRequest : BaseRequest
    {
        /// <summary>
        /// Object used to upload image or file
        /// body takes the payload info
        /// payload take the file/image binary stream content
        /// </summary>
        /// <param name="apiController"></param>
        /// <param name="actionMethod"></param>
        /// <param name="headers"></param>
        /// <param name="queryParams"></param>
        /// <param name="file"></param>
        public BasePostFileRequest(string apiController, string actionMethod, object queryParams, File file,
            List<KeyValuePair<string, string>> headers = null)
            : base(apiController, actionMethod, headers)
        {
            File = file;
            QueryParams = queryParams;

        }

        public File File { get; }
        public object QueryParams { get; }

    }
    public class BasePutRequest : BaseRequest
    {
        public BasePutRequest(string apiController, string actionMethod, object body, List<KeyValuePair<string, string>> headers = null)
            : base(apiController, actionMethod, headers)
        {
            Body = body;
        }
        public object Body { get; }
    }
    public class BaseDeleteRequest : BaseRequest
    {
        public BaseDeleteRequest(string apiController, string actionMethod, object body, List<KeyValuePair<string, string>> headers = null)
            : base(apiController, actionMethod, headers)
        {
            Body = body;
        }
        public object Body { get; }
    }
}