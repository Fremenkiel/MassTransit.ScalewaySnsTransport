namespace MassTransit.ScalewaySnsTransport.Configuration
{
    using Topology;


    public class ScalewaySnsTopicSubscriptionConfigurator :
        ScalewaySnsTopicConfigurator,
        IScalewaySnsTopicSubscriptionConfigurator
    {
        public ScalewaySnsTopicSubscriptionConfigurator(string topicName, bool durable = true, bool autoDelete = false)
            : base(topicName, durable, autoDelete)
        {
        }

        public ScalewaySnsTopicSubscriptionConfigurator(Topic topic)
            : base(topic)
        {
        }
    }
}
