namespace MassTransit.ScalewaySnsTransport.Topology
{
    public class PublishEndpointBrokerTopologyBuilder :
        BrokerTopologyBuilder,
        IPublishEndpointBrokerTopologyBuilder
    {
        /// <summary>
        /// The exchange to which the published message is sent
        /// </summary>
        public TopicHandle Topic { get; set; }

        public BrokerTopology BuildBrokerTopology()
        {
            return new ScalewaySnsBrokerTopology(Topics, Queues, QueueSubscriptions, TopicSubscriptions);
        }
    }
}
