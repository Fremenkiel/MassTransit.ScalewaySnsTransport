namespace MassTransit.ScalewaySnsTransport.Topology
{
    public class ReceiveEndpointBrokerTopologyBuilder :
        BrokerTopologyBuilder,
        IReceiveEndpointBrokerTopologyBuilder
    {
        public QueueHandle Queue { get; set; }

        public BrokerTopology BuildTopologyLayout()
        {
            return new ScalewaySnsBrokerTopology(Topics, Queues, QueueSubscriptions, TopicSubscriptions);
        }
    }
}
