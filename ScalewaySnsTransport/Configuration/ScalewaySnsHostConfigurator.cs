namespace MassTransit.ScalewaySnsTransport.Configuration
{
    using System;
    using Amazon;
    using Amazon.Runtime;
    using Amazon.SimpleNotificationService;
    using Amazon.SQS;
    using Transports;


    public class ScalewaySnsHostConfigurator :
        IScalewaySnsHostConfigurator
    {
        readonly ConfigurationHostSettings _settings;

        string _sqsAccessKey;
        string _sqsSecretKey;

        string _snsAccessKey;
        string _snsSecretKey;

        public ScalewaySnsHostConfigurator(Uri address, string region)
        {
            var hostAddress = new ScalewaySnsHostAddress(address);

            var sqsHost = $"{hostAddress.Scheme}://{hostAddress.Host.Replace("sns", "sqs")}";
            var snsHost = $"{hostAddress.Scheme}://{hostAddress.Host.Replace("sqs", "sns")}";

            var regionEndpoint = RegionEndpoint.GetBySystemName(region);

            _settings = new ConfigurationHostSettings
            {
                Scope = hostAddress.Scope,
                Region = regionEndpoint,
                ServiceURL = hostAddress.Host.Replace("sns", "sqs"),
                ScalewaySqsConfig = new AmazonSQSConfig {ServiceURL = sqsHost},
                ScalewaySnsConfig = new AmazonSimpleNotificationServiceConfig {ServiceURL = snsHost}
            };
        }

        public ScalewaySnsHostSettings Settings => _settings;

        public void SqsAccessKey(string accessKey)
        {
            _sqsAccessKey = accessKey;
            SetBasicSqsCredentials();
        }

        public void SnsAccessKey(string accessKey)
        {
            _snsAccessKey = accessKey;
            SetBasicSnsCredentials();
        }

        public void Scope(string scope, bool scopeTopics)
        {
            _settings.Scope = scope;

            if (scopeTopics)
                EnableScopedTopics();
        }

        public void EnableScopedTopics()
        {
            _settings.ScopeTopics = true;
        }

        public void SqsSecretKey(string secretKey)
        {
            _sqsSecretKey = secretKey;
            SetBasicSqsCredentials();
        }

        public void SnsSecretKey(string secretKey)
        {
            _snsSecretKey = secretKey;
            SetBasicSnsCredentials();
        }

        public void SqsCredentials(AWSCredentials credentials)
        {
            _settings.SqsCredentials = credentials;
        }

        public void SnsCredentials(AWSCredentials credentials)
        {
            _settings.SnsCredentials = credentials;
        }

        public void Config(AmazonSQSConfig config)
        {
            _settings.ScalewaySqsConfig = config;
        }

        public void Config(AmazonSimpleNotificationServiceConfig config)
        {
            _settings.ScalewaySnsConfig = config;
        }

        public void AllowTransportHeader(AllowTransportHeader allowTransportHeader)
        {
            _settings.AllowTransportHeader = allowTransportHeader;
        }

        void SetBasicSqsCredentials()
        {
            if (string.IsNullOrEmpty(_sqsAccessKey) || string.IsNullOrEmpty(_sqsSecretKey))
                return;

            _settings.SqsCredentials = new BasicAWSCredentials(_sqsAccessKey, _sqsSecretKey);
        }

        void SetBasicSnsCredentials()
        {
            if (string.IsNullOrEmpty(_snsAccessKey) || string.IsNullOrEmpty(_snsSecretKey))
                return;

            _settings.SnsCredentials = new BasicAWSCredentials(_snsAccessKey, _snsSecretKey);
        }
    }
}
