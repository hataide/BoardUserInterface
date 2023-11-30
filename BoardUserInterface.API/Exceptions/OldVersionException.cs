using System.Diagnostics.CodeAnalysis;

namespace BoardUserInterface.API.Exceptions;
[ExcludeFromCodeCoverage]
[Serializable]
public class OldVersionException : Exception
{
    public OldVersionException() : base() { }
    public OldVersionException(string message) : base(message) { }
    public OldVersionException(string message, Exception inner) : base(message, inner) { }

    // A constructor is needed for serialization when an
    // exception propagates from a remoting server to the client.
    protected OldVersionException(System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}