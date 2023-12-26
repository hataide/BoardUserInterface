using System.Diagnostics.CodeAnalysis;
using System.Net;

namespace BoardUserInterface.Common.Exceptions;
[ExcludeFromCodeCoverage]
[Serializable]
public class HttpRequestFailedException : Exception
{
    public HttpRequestFailedException() : base() { }
    public HttpRequestFailedException(string message) : base(message) { }
    public HttpRequestFailedException(string message, Exception inner) : base(message, inner) { }

    public HttpRequestFailedException(HttpStatusCode statusCode, string responseContent)
            : base($"Request failed with status code: {statusCode}, Content: {responseContent}") { }

    // A constructor is needed for serialization when an
    // exception propagates from a remoting server to the client.
    protected HttpRequestFailedException(System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}


