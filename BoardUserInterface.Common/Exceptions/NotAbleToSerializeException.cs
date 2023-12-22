using System.Diagnostics.CodeAnalysis;

namespace BoardUserInterface.Common.Exceptions;
[ExcludeFromCodeCoverage]
[Serializable]
public class NotAbleToSerializeException : Exception
{
    public NotAbleToSerializeException() : base() { }
    public NotAbleToSerializeException(string message) : base(message) { }
    public NotAbleToSerializeException(string message, Exception inner) : base(message, inner) { }

    // A constructor is needed for serialization when an
    // exception propagates from a remoting server to the client.
    protected NotAbleToSerializeException(System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}