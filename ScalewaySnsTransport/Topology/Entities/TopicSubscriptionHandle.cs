namespace MassTransit.ScalewaySnsTransport.Topology
{
    using MassTransit.Topology;


    public interface TopicSubscriptionHandle :
        EntityHandle
    {
        TopicSubscription TopicSubscription { get; }
    }
}
