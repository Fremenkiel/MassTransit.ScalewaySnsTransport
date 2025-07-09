namespace MassTransit.ScalewaySnsTransport.Configuration
{
    using System;
    using System.Collections.Generic;
    using Amazon;
    using Amazon.Runtime;
    using Amazon.Runtime.Credentials;
    using MassTransit.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Options;
    using Transports;


    public class ScalewaySnsRegistrationBusFactory :
        TransportRegistrationBusFactory<IScalewaySnsReceiveEndpointConfigurator>
    {
        readonly ScalewaySnsBusConfiguration _busConfiguration;
        readonly Action<IBusRegistrationContext, IScalewaySnsBusFactoryConfigurator> _configure;

        public ScalewaySnsRegistrationBusFactory(Action<IBusRegistrationContext, IScalewaySnsBusFactoryConfigurator> configure)
            : this(new ScalewaySnsBusConfiguration(new ScalewaySnsTopologyConfiguration(ScalewaySnsBusFactory.CreateMessageTopology())), configure)
        {
        }

        ScalewaySnsRegistrationBusFactory(ScalewaySnsBusConfiguration busConfiguration,
            Action<IBusRegistrationContext, IScalewaySnsBusFactoryConfigurator> configure)
            : base(busConfiguration.HostConfiguration)
        {
            _configure = configure;

            _busConfiguration = busConfiguration;
        }

        public override IBusInstance CreateBus(IBusRegistrationContext context, IEnumerable<IBusInstanceSpecification> specifications, string busName)
        {
            var configurator = new ScalewaySnsBusFactoryConfigurator(_busConfiguration);

            var options = context.GetRequiredService<IOptionsMonitor<ScalewaySnsTransportOptions>>().Get(busName);

            if (!string.IsNullOrWhiteSpace(options.Region))
            {
                var regionEndpoint = RegionEndpoint.GetBySystemName(options.Region);

                configurator.Host(regionEndpoint.SystemName, h =>
                {
                    if (!string.IsNullOrWhiteSpace(options.Scope))
                        h.Scope(options.Scope);

                    if (!string.IsNullOrWhiteSpace(options.SqsAccessKey) && !string.IsNullOrWhiteSpace(options.SqsSecretKey)
                        && !string.IsNullOrWhiteSpace(options.SnsAccessKey) && !string.IsNullOrWhiteSpace(options.SnsSecretKey))
                    {
                        h.SqsAccessKey(options.SqsAccessKey);
                        h.SqsSecretKey(options.SqsSecretKey);
                        h.SnsAccessKey(options.SnsAccessKey);
                        h.SnsSecretKey(options.SnsSecretKey);
                    }
                    else
                    {
                        h.SqsCredentials(DefaultAWSCredentialsIdentityResolver.GetCredentials());
                        h.SnsCredentials(DefaultAWSCredentialsIdentityResolver.GetCredentials());
                    }
                });
            }

            return CreateBus(configurator, context, _configure, specifications);
        }
    }
}
