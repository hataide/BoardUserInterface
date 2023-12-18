using System.Diagnostics.CodeAnalysis;

namespace BoardUserInterface.Common.Exceptions;
[ExcludeFromCodeCoverage]
[Serializable]
public class InvalidFileExtensionException : Exception
{
    public InvalidFileExtensionException() : base() { }
    public InvalidFileExtensionException(string message) : base(message) { }
    public InvalidFileExtensionException(string message, Exception inner) : base(message, inner) { }

    // A constructor is needed for serialization when an
    // exception propagates from a remoting server to the client.
    protected InvalidFileExtensionException(System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}