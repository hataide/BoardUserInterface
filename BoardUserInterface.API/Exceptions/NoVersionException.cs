﻿using System.Diagnostics.CodeAnalysis;

namespace BoardUserInterface.API.Exceptions;
[ExcludeFromCodeCoverage]
[Serializable]
public class NoVersionException : Exception
{
    public NoVersionException() : base() { }
    public NoVersionException(string message) : base(message) { }
    public NoVersionException(string message, Exception inner) : base(message, inner) { }

    // A constructor is needed for serialization when an
    // exception propagates from a remoting server to the client.
    protected NoVersionException(System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}