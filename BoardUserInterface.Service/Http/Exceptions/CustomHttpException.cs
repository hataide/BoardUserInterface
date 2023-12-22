using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BoardUserInterface.Service.Http.Exceptions
{
    //internal class CustomHttpException
    public class HttpRequestFailedException : Exception
    {
        public HttpStatusCode StatusCode { get; }
        public string ResponseContent { get; }

        public HttpRequestFailedException(HttpStatusCode statusCode, string responseContent)
            : base($"Request failed with status code: {statusCode}, Content: {responseContent}")
        {
            StatusCode = statusCode;
            ResponseContent = responseContent;
        }
    }

    public class JsonDeserializationException : Exception
    {
        public JsonDeserializationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }

    public class ServiceEndpointNotFoundException : Exception
    {
        public ServiceEndpointNotFoundException(string message) : base(message)
        {
        }
    }
}

