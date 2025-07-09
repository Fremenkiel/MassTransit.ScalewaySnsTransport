namespace MassTransit.ScalewaySnsTransport.Configuration
{
    using System;
    using Amazon;
    using Amazon.Runtime;
    using Amazon.SimpleNotificationService;
    using Amazon.SQS;
    using Transports;


    public class ConfigurationHostSettings :
        ScalewaySnsHostSettings
    {
        readonly Lazy<Uri> _hostAddress;
        AWSCredentials _sqsCredentials;
        AWSCredentials _snsCredentials;
        ImmutableCredentials _sqsImmutableCredentials;
        ImmutableCredentials _snsImmutableCredentials;

        public ConfigurationHostSettings()
        {
            _hostAddress = new Lazy<Uri>(FormatHostAddress);
        }

        public AWSCredentials SqsCredentials
        {
            get => _sqsCredentials;
            set
            {
                _sqsCredentials = value;
                _sqsImmutableCredentials = null;
            }
        }

        public AWSCredentials SnsCredentials
        {
            get => _snsCredentials;
            set
            {
                _snsCredentials = value;
                _snsImmutableCredentials = null;
            }
        }

        public AmazonSQSConfig ScalewaySqsConfig { get; set; }

        public AmazonSimpleNotificationServiceConfig ScalewaySnsConfig { get; set; }

        public string Scope { get; set; }

        public string ServiceURL { get; set; }
        public RegionEndpoint Region { get; set; }
        public string SqsAccessKey => (_sqsImmutableCredentials ??= GetSqsImmutableCredentials()).AccessKey;
        public string SnsAccessKey => (_snsImmutableCredentials ??= GetSnsImmutableCredentials()).AccessKey;
        public string SqsSecretKey => (_sqsImmutableCredentials ??= GetSqsImmutableCredentials()).SecretKey;
        public string SnsSecretKey => (_snsImmutableCredentials ??= GetSnsImmutableCredentials()).SecretKey;

        public AllowTransportHeader AllowTransportHeader { get; set; }

        public bool ScopeTopics { get; set; }

        public Uri HostAddress => _hostAddress.Value;

        public IConnection CreateConnection()
        {
            return new Connection(SqsCredentials, SnsCredentials, Region, ScalewaySqsConfig, ScalewaySnsConfig);
        }

        Uri FormatHostAddress()
        {
            return new ScalewaySnsHostAddress(ServiceURL, Scope);
        }

        public override string ToString()
        {
            return new UriBuilder
            {
                Scheme = "https",
                Host = ServiceURL
            }.Uri.ToString();
        }

        ImmutableCredentials GetSqsImmutableCredentials()
        {
            return SqsCredentials?.GetCredentials() ?? throw new ArgumentNullException(nameof(SqsCredentials));
        }

        ImmutableCredentials GetSnsImmutableCredentials()
        {
            return SnsCredentials?.GetCredentials() ?? throw new ArgumentNullException(nameof(SnsCredentials));
        }
    }
}
