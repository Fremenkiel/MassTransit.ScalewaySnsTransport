namespace MassTransit
{
    using System;
    using System.Runtime.Serialization;


    [Serializable]
    public class ScalewaySnsTransportConfigurationException :
        ScalewaySnsTransportException
    {
        public ScalewaySnsTransportConfigurationException()
        {
        }

        public ScalewaySnsTransportConfigurationException(string message)
            : base(message)
        {
        }

        public ScalewaySnsTransportConfigurationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

#if NET8_0_OR_GREATER
        [Obsolete("Formatter-based serialization is obsolete and should not be used.")]
#endif
        protected ScalewaySnsTransportConfigurationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
