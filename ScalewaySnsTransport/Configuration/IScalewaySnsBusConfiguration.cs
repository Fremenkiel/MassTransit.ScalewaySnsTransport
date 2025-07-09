namespace MassTransit.ScalewaySnsTransport.Configuration
{
    using MassTransit.Configuration;


    public interface IScalewaySnsBusConfiguration :
        IBusConfiguration
    {
        new IScalewaySnsHostConfiguration HostConfiguration { get; }

        new IScalewaySnsEndpointConfiguration BusEndpointConfiguration { get; }

        new IScalewaySnsTopologyConfiguration Topology { get; }

        /// <summary>
        /// Create an endpoint configuration on the bus, which can later be turned into a receive endpoint
        /// </summary>
        /// <returns></returns>
        IScalewaySnsEndpointConfiguration CreateEndpointConfiguration(bool isBusEndpoint = false);
    }
}
