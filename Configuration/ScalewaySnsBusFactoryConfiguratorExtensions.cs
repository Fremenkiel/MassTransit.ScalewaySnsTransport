#nullable enable
namespace MassTransit
{
    using System;
    using Amazon;
    using Amazon.Runtime;
    using Amazon.Runtime.Credentials;
    using Amazon.SimpleNotificationService;
    using Amazon.SQS;
    using ScalewaySnsTransport.Configuration;


    public static class ScalewaySnsBusFactoryConfiguratorExtensions
    {
        /// <summary>
        /// Select ScalewaySNS as the transport for the service bus
        /// </summary>
        public static IBusControl CreateUsingScalewaySns(this IBusFactorySelector selector, Action<IScalewaySnsBusFactoryConfigurator> configure)
        {
            return ScalewaySnsBusFactory.Create(configure);
        }

        /// <summary>
        /// Configure MassTransit to use ScalewaySns for the transport.
        /// </summary>
        /// <param name="configurator">The registration configurator (configured via AddMassTransit)</param>
        /// <param name="configure">The configuration callback for the bus factory</param>
        public static void UsingScalewaySns(this IBusRegistrationConfigurator configurator,
            Action<IBusRegistrationContext, IScalewaySnsBusFactoryConfigurator>? configure = null)
        {
            configurator.SetBusFactory(new ScalewaySnsRegistrationBusFactory(configure));
        }

        /// <summary>
        /// Configure the transport to use Localstack (hosted in Docker, on the default port of 4566
        /// </summary>
        /// <param name="configurator"></param>
        public static void LocalstackHost(this IScalewaySnsBusFactoryConfigurator configurator)
        {
            configurator.Host(new Uri("scalewaysns://localhost:4566"), h =>
            {
                h.SqsAccessKey("admin");
                h.SqsSecretKey("admin");

                h.SnsAccessKey("admin");
                h.SnsSecretKey("admin");

                h.Config(new AmazonSimpleNotificationServiceConfig { ServiceURL = "http://localhost:4566" });
            });
        }

        /// <summary>
        /// Configure the default Amazon SQS Host, using the FallbackRegionFactory and FallbackCredentialsFactory
        /// </summary>
        /// <param name="configurator"></param>
        /// <param name="configure"></param>
        public static void UseDefaultHost(this IScalewaySnsBusFactoryConfigurator configurator, Action<IScalewaySnsHostConfigurator>? configure = null)
        {
            configurator.UseDefaultHost(FallbackRegionFactory.GetRegionEndpoint(), configure);
        }

        /// <summary>
        /// Configure the default Amazon SQS Host, using the FallbackCredentialsFactory with the specified region
        /// </summary>
        /// <param name="configurator"></param>
        /// <param name="endpoint">The region for the host</param>
        /// <param name="configure"></param>
        public static void UseDefaultHost(this IScalewaySnsBusFactoryConfigurator configurator, RegionEndpoint endpoint,
            Action<IScalewaySnsHostConfigurator>? configure = null)
        {
            configurator.Host(endpoint.SystemName, h =>
            {
                h.SqsCredentials(DefaultAWSCredentialsIdentityResolver.GetCredentials());
                h.SnsCredentials(DefaultAWSCredentialsIdentityResolver.GetCredentials());

                configure?.Invoke(h);
            });
        }
    }
}
