namespace MassTransit
{
    using System;
    using System.Runtime.Serialization;


    [Serializable]
    public class ScalewaySnsConnectException :
        ScalewaySnsConnectionException
    {
        public ScalewaySnsConnectException()
        {
        }

        public ScalewaySnsConnectException(string message)
            : base(message)
        {
        }

        public ScalewaySnsConnectException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

#if NET8_0_OR_GREATER
        [Obsolete("Formatter-based serialization is obsolete and should not be used.")]
#endif
        protected ScalewaySnsConnectException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
