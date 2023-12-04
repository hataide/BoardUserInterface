using System.Diagnostics.CodeAnalysis;

namespace BoardUserInterface.API.Exceptions;
[ExcludeFromCodeCoverage]
[Serializable]
public class RemoveFileException : Exception
{
    public RemoveFileException() : base() { }
    public RemoveFileException(string message) : base(message) { }
    public RemoveFileException(string message, Exception inner) : base(message, inner) { }

    // A constructor is needed for serialization when an
    // exception propagates from a remoting server to the client.
    protected RemoveFileException(System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}