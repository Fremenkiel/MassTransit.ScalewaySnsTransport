namespace MassTransit.ScalewaySnsTransport.Topology
{
    using MassTransit.Topology;


    public interface TopicHandle :
        EntityHandle
    {
        Topic Topic { get; }
    }
}
