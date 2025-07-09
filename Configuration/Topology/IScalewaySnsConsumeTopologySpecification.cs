namespace MassTransit
{
    using ScalewaySnsTransport.Topology;


    public interface IScalewaySnsConsumeTopologySpecification :
        ISpecification
    {
        void Apply(IReceiveEndpointBrokerTopologyBuilder builder);
    }
}
