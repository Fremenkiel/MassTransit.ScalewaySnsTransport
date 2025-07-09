namespace MassTransit.ScalewaySnsTransport.Configuration
{
    using MassTransit.Configuration;


    public class ScalewaySnsEndpointConfiguration :
        EndpointConfiguration,
        IScalewaySnsEndpointConfiguration
    {
        public ScalewaySnsEndpointConfiguration(IScalewaySnsTopologyConfiguration topologyConfiguration)
            : base(topologyConfiguration)
        {
            Topology = topologyConfiguration;
        }

        ScalewaySnsEndpointConfiguration(IEndpointConfiguration parentConfiguration, IScalewaySnsTopologyConfiguration topologyConfiguration, bool isBusEndpoint)
            : base(parentConfiguration, topologyConfiguration, isBusEndpoint)
        {
            Topology = topologyConfiguration;
        }

        public new IScalewaySnsTopologyConfiguration Topology { get; }

        public IScalewaySnsEndpointConfiguration CreateEndpointConfiguration(bool isBusEndpoint)
        {
            var topologyConfiguration = new ScalewaySnsTopologyConfiguration(Topology);

            return new ScalewaySnsEndpointConfiguration(this, topologyConfiguration, isBusEndpoint);
        }
    }
}
