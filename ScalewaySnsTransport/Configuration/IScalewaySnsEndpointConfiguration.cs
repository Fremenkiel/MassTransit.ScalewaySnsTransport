namespace MassTransit.ScalewaySnsTransport.Configuration
{
    using MassTransit.Configuration;


    public interface IScalewaySnsEndpointConfiguration :
        IEndpointConfiguration
    {
        new IScalewaySnsTopologyConfiguration Topology { get; }
    }
}
