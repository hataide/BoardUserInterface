using System.Diagnostics.CodeAnalysis;

namespace BoardUserInterface.Common.Exceptions;
[ExcludeFromCodeCoverage]
[Serializable]
public class NotAJsonException : Exception
{
    public NotAJsonException() : base() { }
    public NotAJsonException(string message) : base(message) { }
    public NotAJsonException(string message, Exception inner) : base(message, inner) { }

    // A constructor is needed for serialization when an
    // exception propagates from a remoting server to the client.
    protected NotAJsonException(System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}