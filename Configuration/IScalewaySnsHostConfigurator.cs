namespace MassTransit
{
    using Amazon.Runtime;
    using Amazon.SimpleNotificationService;
    using Amazon.SQS;
    using Transports;


    public interface IScalewaySnsHostConfigurator
    {
        /// <summary>
        /// Sets the accessKey for the connection to ScalewaySQS
        /// </summary>
        /// <param name="accessKey"></param>
        void SqsAccessKey(string accessKey);

        /// <summary>
        /// Sets the accessKey for the connection to ScalewaySNS
        /// </summary>
        /// <param name="accessKey"></param>
        void SnsAccessKey(string accessKey);

        /// <summary>
        /// Sets the secretKey for the connection to ScalewaySqs
        /// </summary>
        /// <param name="secretKey"></param>
        void SqsSecretKey(string secretKey);

        /// <summary>
        /// Sets the secretKey for the connection to ScalewaySns
        /// </summary>
        /// <param name="secretKey"></param>
        void SnsSecretKey(string secretKey);

        /// <summary>
        /// Sets the credentials for the connection to ScalewaySns
        /// This is an alternative to using AccessKey() and SecretKey()
        /// </summary>
        /// <param name="credentials"></param>
        void SqsCredentials(AWSCredentials credentials);

        /// <summary>
        /// Sets the credentials for the connection to ScalewaySns
        /// This is an alternative to using AccessKey() and SecretKey()
        /// </summary>
        /// <param name="credentials"></param>
        void SnsCredentials(AWSCredentials credentials);

        /// <summary>
        /// Set scope for ScalewaySNS. Will be used as a prefix for queue/topic name
        /// </summary>
        /// <param name="scope"></param>
        /// <param name="scopeTopics">If true, topics will be scoped to the host scope</param>
        void Scope(string scope, bool scopeTopics = false);

        /// <summary>
        /// Enable the scoping of topics to use the host scope (specified via the <see cref="Scope"/> method.
        /// </summary>
        void EnableScopedTopics();

        /// <summary>
        /// Sets the default config for the connection to AmazonSNS
        /// </summary>
        /// <param name="config"></param>
        void Config(AmazonSimpleNotificationServiceConfig config);

        /// <summary>
        /// Specifies a method used to determine if a header should be copied to the transport message
        /// </summary>
        /// <param name="allowTransportHeader"></param>
        void AllowTransportHeader(AllowTransportHeader allowTransportHeader);
    }
}
