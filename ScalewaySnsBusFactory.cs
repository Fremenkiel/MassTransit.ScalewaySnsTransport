namespace MassTransit
{
    using System;
    using ScalewaySnsTransport;
    using ScalewaySnsTransport.Configuration;
    using Configuration;
    using Topology;


    public static class ScalewaySnsBusFactory
    {
        /// <summary>
        /// Configure and create a bus for ScalewaySns
        /// </summary>
        /// <param name="configure">The configuration callback to configure the bus</param>
        /// <returns></returns>
        public static IBusControl Create(Action<IScalewaySnsBusFactoryConfigurator> configure)
        {
            var topologyConfiguration = new ScalewaySnsTopologyConfiguration(CreateMessageTopology());
            var busConfiguration = new ScalewaySnsBusConfiguration(topologyConfiguration);

            var configurator = new ScalewaySnsBusFactoryConfigurator(busConfiguration);

            configure(configurator);

            return configurator.Build(busConfiguration);
        }

        public static IMessageTopologyConfigurator CreateMessageTopology()
        {
            return new MessageTopology(Cached.EntityNameFormatter);
        }


        static class Cached
        {
            internal static readonly IEntityNameFormatter EntityNameFormatter;

            static Cached()
            {
                EntityNameFormatter = new MessageNameFormatterEntityNameFormatter(new ScalewaySnsMessageNameFormatter());
            }
        }
    }
}
