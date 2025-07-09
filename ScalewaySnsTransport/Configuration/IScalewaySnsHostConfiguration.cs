namespace MassTransit.ScalewaySnsTransport.Configuration
{
    using System;
    using MassTransit.Configuration;
    using Topology;


    public interface IScalewaySnsHostConfiguration :
        IHostConfiguration,
        IReceiveConfigurator<IScalewaySnsReceiveEndpointConfigurator>
    {
        ScalewaySnsHostSettings Settings { get; set; }

        IConnectionContextSupervisor ConnectionContextSupervisor { get; }

        new IScalewaySnsBusTopology Topology { get; }

        /// <summary>
        /// Apply the endpoint definition to the receive endpoint configurator
        /// </summary>
        /// <param name="configurator"></param>
        /// <param name="definition"></param>
        void ApplyEndpointDefinition(IScalewaySnsReceiveEndpointConfigurator configurator, IEndpointDefinition definition);

        /// <summary>
        /// Create a receive endpoint configuration using the specified host
        /// </summary>
        /// <returns></returns>
        IScalewaySnsReceiveEndpointConfiguration CreateReceiveEndpointConfiguration(string queueName,
            Action<IScalewaySnsReceiveEndpointConfigurator> configure = null);

        /// <summary>
        /// Create a receive endpoint configuration for the default host
        /// </summary>
        /// <param name="settings"></param>
        /// <param name="endpointConfiguration"></param>
        /// <param name="configure"></param>
        /// <returns></returns>
        IScalewaySnsReceiveEndpointConfiguration CreateReceiveEndpointConfiguration(QueueReceiveSettings settings,
            IScalewaySnsEndpointConfiguration endpointConfiguration, Action<IScalewaySnsReceiveEndpointConfigurator> configure = null);
    }
}
