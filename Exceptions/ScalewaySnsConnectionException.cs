namespace MassTransit
{
    using System;
    using System.Runtime.Serialization;


    [Serializable]
    public class ScalewaySnsConnectionException :
        ConnectionException
    {
        public ScalewaySnsConnectionException()
        {
        }

        public ScalewaySnsConnectionException(string message)
            : base(message)
        {
        }

        public ScalewaySnsConnectionException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

#if NET8_0_OR_GREATER
        [Obsolete("Formatter-based serialization is obsolete and should not be used.")]
#endif
        protected ScalewaySnsConnectionException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
