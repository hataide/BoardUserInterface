using System.Diagnostics.CodeAnalysis;

namespace BoardUserInterface.Common.Exceptions;
[ExcludeFromCodeCoverage]
[Serializable]
public class InvalidFileContentTypeException : Exception
{
    public InvalidFileContentTypeException() : base() { }
    public InvalidFileContentTypeException(string message) : base(message) { }
    public InvalidFileContentTypeException(string message, Exception inner) : base(message, inner) { }

    // A constructor is needed for serialization when an
    // exception propagates from a remoting server to the client.
    protected InvalidFileContentTypeException(System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}