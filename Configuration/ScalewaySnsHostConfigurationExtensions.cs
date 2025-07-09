namespace MassTransit
{
    using System;
    using ScalewaySnsTransport.Configuration;


    public static class ScalewaySnsHostConfigurationExtensions
    {
        /// <summary>
        /// Configure a ScalewaySNS host using the configuration API
        /// </summary>
        /// <param name="configurator"></param>
        /// <param name="hostAddress">The URI host address of the ScalewaySns host (scalewaysns://region)</param>
        /// <param name="configure"></param>
        public static void Host(this IScalewaySnsBusFactoryConfigurator configurator, Uri hostAddress, Action<IScalewaySnsHostConfigurator> configure)
        {
            if (hostAddress == null)
                throw new ArgumentNullException(nameof(hostAddress));

            var hostConfigurator = new ScalewaySnsHostConfigurator(hostAddress, configurator.Region);

            configure(hostConfigurator);

            configurator.Host(hostConfigurator.Settings);
        }

        /// <summary>
        /// Configure a ScalewwaySns host with a host name and virtual host
        /// </summary>
        /// <param name="configurator"></param>
        /// <param name="hostName">The host name of the broker</param>
        /// <param name="configure">The configuration callback</param>
        public static void Host(this IScalewaySnsBusFactoryConfigurator configurator, string hostName, Action<IScalewaySnsHostConfigurator> configure)
        {
            configurator.Region = hostName;
            configurator.Host(new UriBuilder("https", $"sqs.mnq.{hostName}.scaleway.com").Uri, configure);
        }

        /// <summary>
        /// Declare a ReceiveEndpoint using a unique generated queue name. This queue defaults to auto-delete
        /// and non-durable. By default all services bus instances include a default receiveEndpoint that is
        /// of this type (created automatically upon the first receiver binding).
        /// </summary>
        /// <param name="configurator"></param>
        /// <param name="configure"></param>
        public static void ReceiveEndpoint(this IScalewaySnsBusFactoryConfigurator configurator, Action<IScalewaySnsReceiveEndpointConfigurator> configure = null)
        {
            configurator.ReceiveEndpoint(new TemporaryEndpointDefinition(), DefaultEndpointNameFormatter.Instance, configure);
        }

        /// <summary>
        /// Declare a receive endpoint using the endpoint <paramref name="definition"/>.
        /// </summary>
        /// <param name="configurator"></param>
        /// <param name="definition"></param>
        /// <param name="configure"></param>
        public static void ReceiveEndpoint(this IScalewaySnsBusFactoryConfigurator configurator, IEndpointDefinition definition,
            Action<IScalewaySnsReceiveEndpointConfigurator> configure = null)
        {
            configurator.ReceiveEndpoint(definition, DefaultEndpointNameFormatter.Instance, configure);
        }
    }
}
