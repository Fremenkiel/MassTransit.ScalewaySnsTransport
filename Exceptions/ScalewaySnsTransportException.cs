namespace MassTransit
{
    using System;
    using System.Runtime.Serialization;


    [Serializable]
    public class ScalewaySnsTransportException :
        MassTransitException
    {
        public ScalewaySnsTransportException()
        {
        }

        public ScalewaySnsTransportException(string message)
            : base(message)
        {
        }

        public ScalewaySnsTransportException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

#if NET8_0_OR_GREATER
        [Obsolete("Formatter-based serialization is obsolete and should not be used.")]
#endif
        protected ScalewaySnsTransportException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
