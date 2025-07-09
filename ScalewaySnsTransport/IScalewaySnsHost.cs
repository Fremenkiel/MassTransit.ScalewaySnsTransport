namespace MassTransit.ScalewaySnsTransport
{
    using Transports;


    public interface IScalewaySnsHost :
        IHost<IScalewaySnsReceiveEndpointConfigurator>
    {
        new IScalewaySnsBusTopology Topology { get; }
    }
}
