using System.Diagnostics.CodeAnalysis;

namespace BoardUserInterface.API.Exceptions;
[ExcludeFromCodeCoverage]
[Serializable]
public class InvalidVersionException : Exception
{
    public InvalidVersionException() : base() { }
    public InvalidVersionException(string message) : base(message) { }
    public InvalidVersionException(string message, Exception inner) : base(message, inner) { }

    // A constructor is needed for serialization when an
    // exception propagates from a remoting server to the client.
    protected InvalidVersionException(System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}