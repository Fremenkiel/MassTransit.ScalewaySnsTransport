namespace MassTransit.ScalewaySnsTransport.Topology
{
    public class SendEndpointBrokerTopologyBuilder :
        BrokerTopologyBuilder,
        ISendEndpointBrokerTopologyBuilder
    {
        /// <summary>
        /// The queue to which messages are sent
        /// </summary>
        public QueueHandle Queue { get; set; }

        public BrokerTopology BuildBrokerTopology()
        {
            return new ScalewaySnsBrokerTopology(Topics, Queues, QueueSubscriptions, TopicSubscriptions);
        }
    }
}
