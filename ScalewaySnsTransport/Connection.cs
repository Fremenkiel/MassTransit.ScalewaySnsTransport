namespace MassTransit.ScalewaySnsTransport
{
    using Amazon;
    using Amazon.Runtime;
    using Amazon.SimpleNotificationService;
    using Amazon.SQS;


    public class Connection :
        IConnection
    {
        public Connection(AWSCredentials sqsCredentials, AWSCredentials snsCredentials, RegionEndpoint regionEndpoint = null, AmazonSQSConfig ScalewaySqsConfig = null,
            AmazonSimpleNotificationServiceConfig scalewaySnsConfig = null)
        {
            ScalewaySqsConfig ??= new AmazonSQSConfig { RegionEndpoint = regionEndpoint ?? RegionEndpoint.USEast1 };
            scalewaySnsConfig ??= new AmazonSimpleNotificationServiceConfig { RegionEndpoint = regionEndpoint ?? RegionEndpoint.USEast1 };


            SqsClient = sqsCredentials == null
                ? new AmazonSQSClient(ScalewaySqsConfig)
                : new AmazonSQSClient(sqsCredentials, ScalewaySqsConfig);


            SnsClient = snsCredentials == null
                ? new AmazonSimpleNotificationServiceClient(scalewaySnsConfig)
                : new AmazonSimpleNotificationServiceClient(snsCredentials, scalewaySnsConfig);
        }

        public IAmazonSQS SqsClient { get; }
        public IAmazonSimpleNotificationService SnsClient { get; }

        public void Dispose()
        {
            SnsClient.Dispose();
            SqsClient.Dispose();
        }
    }
}
