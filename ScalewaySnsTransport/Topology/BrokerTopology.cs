namespace MassTransit.ScalewaySnsTransport.Topology
{
    public interface BrokerTopology :
        IProbeSite
    {
        Topic[] Topics { get; }
        Queue[] Queues { get; }
        QueueSubscription[] QueueSubscriptions { get; }
        TopicSubscription[] TopicSubscriptions { get; }
    }
}
