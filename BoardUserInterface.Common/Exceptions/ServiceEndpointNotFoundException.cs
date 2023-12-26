using System.Diagnostics.CodeAnalysis;

namespace BoardUserInterface.Common.Exceptions;
    [ExcludeFromCodeCoverage]
    [Serializable]
    public class ServiceEndpointNotFoundException : Exception
    {
        public ServiceEndpointNotFoundException() : base() { }
        public ServiceEndpointNotFoundException(string message) : base(message) { }
        public ServiceEndpointNotFoundException(string message, Exception inner) : base(message, inner) { }

        // A constructor is needed for serialization when an
        // exception propagates from a remoting server to the client.
        protected ServiceEndpointNotFoundException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

