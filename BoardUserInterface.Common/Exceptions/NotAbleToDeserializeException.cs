using System.Diagnostics.CodeAnalysis;

namespace BoardUserInterface.Common.Exceptions;
[ExcludeFromCodeCoverage]
[Serializable]
public class NotAbleToDeserializeException : Exception
{
    public NotAbleToDeserializeException() : base() { }
    public NotAbleToDeserializeException(string message) : base(message) { }
    public NotAbleToDeserializeException(string message, Exception inner) : base(message, inner) { }

    // A constructor is needed for serialization when an
    // exception propagates from a remoting server to the client.
    protected NotAbleToDeserializeException(System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}