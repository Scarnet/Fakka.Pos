using System;
using System.Net.Http;
using Fakka.Core.Managers;
using Fakka.Core.Models;

namespace Fakka.Core.Exceptions
{
    public class BaseException : Exception
    {
        public BaseException()
        {
        }

        public BaseException(HttpResponse httpResponse)
        {
            ErrorResponse = new BaseErrorResponse((int)httpResponse.StatusCode,
                ApplicationManager.Instance.GetApplicationInfo().Name,
                httpResponse.ReasonPhrase, string.Empty);
        }

        public BaseErrorResponse ErrorResponse { get; set; }

    }



    public class InternalServerErrorException : BaseException
    {
        public InternalServerErrorException(HttpResponse httpResponse) : base(httpResponse)
        {

        }
    }

    public class NetworkException : BaseException
    {
        public NetworkException(HttpResponse httpResponse) : base(httpResponse)
        {
        }
    }



    public class AuthorizationException : BaseException
    {
        public AuthorizationException(HttpResponse httpResponse) : base(httpResponse)
        {

        }
    }

    public class BusinessException : BaseException
    {
        public BusinessException(int errorCode, string errorMessage, string errorDetails)
        {
            ErrorResponse = new BaseErrorResponse(errorCode,
                ApplicationManager.Instance.GetApplicationInfo().Name,
                errorMessage, errorDetails);
        }
    }

}
