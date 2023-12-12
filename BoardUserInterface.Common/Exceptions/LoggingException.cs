using System.Diagnostics.CodeAnalysis;

namespace BoardUserInterface.Common.Exceptions;
[ExcludeFromCodeCoverage]
[Serializable]
public class LoggingException : Exception
{
    public LoggingException() : base() { }
    public LoggingException(string message) : base(message) { }
    public LoggingException(string message, Exception inner) : base(message, inner) { }

    // A constructor is needed for serialization when an
    // exception propagates from a remoting server to the client.
    protected LoggingException(System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}