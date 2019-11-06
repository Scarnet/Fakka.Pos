using System;

namespace Fakka.Core.Models
{
    /// <summary>
    /// Object returned in case of success
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BaseResponse<T>
    {
        public BaseResponse(BaseServerResponse<T> response)
        {
            SuccessCode = response.ErrorCode;
            Data = response.Data;
            //AccessToken = response.AccessToken;

        }

        public T Data { get; set; }
        public int SuccessCode { get; set; }
        //public string AccessToken { get; }
    }
    
    /// <summary>
    /// Object thrown in exception in case of error
    /// </summary>
   public class BaseErrorResponse
    {
        public BaseErrorResponse()
        {
        }

        public BaseErrorResponse(int errorCode, string errorTitle, string errorMessage, string errorDetails, Object responseData = null)
        {
            ErrorCode = errorCode;
            ErrorTitle = errorTitle;
            ErrorMessage = errorMessage;
            ErrorDetails = errorDetails;
            ResponseData = responseData;
        }
        public int ErrorCode { get; set; }
        public string ErrorTitle { get; set; }
        public string ErrorMessage { get; }
        public string ErrorDetails { get; set; }
        public Object ResponseData { get; }
    }
}