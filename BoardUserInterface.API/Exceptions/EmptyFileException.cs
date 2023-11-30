using System.Diagnostics.CodeAnalysis;

namespace BoardUserInterface.API.Exceptions;
[ExcludeFromCodeCoverage]
[Serializable]
public class EmptyFileException : Exception
{
    public EmptyFileException() : base() { }
    public EmptyFileException(string message) : base(message) { }
    public EmptyFileException(string message, Exception inner) : base(message, inner) { }

    // A constructor is needed for serialization when an
    // exception propagates from a remoting server to the client.
    protected EmptyFileException(System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}