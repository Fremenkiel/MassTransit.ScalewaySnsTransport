namespace MassTransit.ScalewaySnsTransport.Configuration
{
    using System.Collections.Generic;
    using System.Linq;
    using Internals;
    using Topology;


    /// <summary>
    /// Used to by a TopicSubscription destination to the receive endpoint, via an additional message consumer
    /// </summary>
    public class ConsumerConsumeTopologySpecification :
        ScalewaySnsTopicSubscriptionConfigurator,
        IScalewaySnsConsumeTopologySpecification
    {
        readonly IScalewaySnsPublishTopology _publishTopology;

        public ConsumerConsumeTopologySpecification(IScalewaySnsPublishTopology publishTopology, string topicName, bool durable = true, bool autoDelete = false)
            : base(topicName, durable, autoDelete)
        {
            _publishTopology = publishTopology;
        }

        public ConsumerConsumeTopologySpecification(IScalewaySnsPublishTopology publishTopology, Topic topic)
            : base(topic)
        {
            _publishTopology = publishTopology;
        }

        public IEnumerable<ValidationResult> Validate()
        {
            return Enumerable.Empty<ValidationResult>();
        }

        public void Apply(IReceiveEndpointBrokerTopologyBuilder builder)
        {
            var topicHandle = builder.CreateTopic(EntityName, Durable, AutoDelete,
                _publishTopology.TopicAttributes.MergeLeft(TopicAttributes),
                _publishTopology.TopicSubscriptionAttributes.MergeLeft(TopicSubscriptionAttributes),
                _publishTopology.TopicTags.MergeLeft(Tags));


            var topicSubscriptionHandle = builder.CreateQueueSubscription(topicHandle, builder.Queue);
        }
    }
}
