#nullable enable
namespace MassTransit.ScalewaySnsTransport.Topology
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Configuration;
    using Internals;
    using MassTransit.Topology;


    public class ScalewaySnsMessagePublishTopology<TMessage> :
        MessagePublishTopology<TMessage>,
        IScalewaySnsMessagePublishTopologyConfigurator<TMessage>
        where TMessage : class
    {
        readonly ScalewaySnsTopicConfigurator _scalewaySnsTopic;
        readonly IScalewaySnsPublishTopology _publishTopology;

        public ScalewaySnsMessagePublishTopology(IScalewaySnsPublishTopology publishTopology, IMessageTopology<TMessage> messageTopology)
            : base(publishTopology)
        {
            _publishTopology = publishTopology;

            var topicName = messageTopology.EntityName;

            var temporary = MessageTypeCache<TMessage>.IsTemporaryMessageType;

            var durable = !temporary;
            var autoDelete = temporary;

            _scalewaySnsTopic = new ScalewaySnsTopicConfigurator(topicName, durable, autoDelete);
        }

        public Topic Topic => _scalewaySnsTopic;

        bool IScalewaySnsTopicConfigurator.Durable
        {
            set => _scalewaySnsTopic.Durable = value;
        }

        bool IScalewaySnsTopicConfigurator.AutoDelete
        {
            set => _scalewaySnsTopic.AutoDelete = value;
        }

        IDictionary<string, object> IScalewaySnsTopicConfigurator.TopicAttributes => _scalewaySnsTopic.TopicAttributes;
        IDictionary<string, object> IScalewaySnsTopicConfigurator.TopicSubscriptionAttributes => _scalewaySnsTopic.TopicSubscriptionAttributes;
        IDictionary<string, string> IScalewaySnsTopicConfigurator.TopicTags => _scalewaySnsTopic.TopicTags;

        public ScalewaySnsEndpointAddress GetEndpointAddress(Uri hostAddress)
        {
            return _scalewaySnsTopic.GetEndpointAddress(hostAddress);
        }

        public override bool TryGetPublishAddress(Uri baseAddress, [NotNullWhen(true)] out Uri? publishAddress)
        {
            publishAddress = _scalewaySnsTopic.GetEndpointAddress(baseAddress);
            return true;
        }

        public void Apply(IPublishEndpointBrokerTopologyBuilder builder)
        {
            if (Exclude)
                return;

            var topicHandle = builder.CreateTopic(_scalewaySnsTopic.EntityName, _scalewaySnsTopic.Durable, _scalewaySnsTopic.AutoDelete,
                _publishTopology.TopicAttributes.MergeLeft(_scalewaySnsTopic.TopicAttributes),
                _publishTopology.TopicSubscriptionAttributes.MergeLeft(_scalewaySnsTopic.TopicSubscriptionAttributes),
                _publishTopology.TopicTags.MergeLeft(_scalewaySnsTopic.Tags));

            builder.Topic ??= topicHandle;
        }

        public PublishSettings GetPublishSettings(Uri hostAddress)
        {
            return new TopicPublishSettings(GetEndpointAddress(hostAddress));
        }

        public BrokerTopology GetBrokerTopology()
        {
            var builder = new PublishEndpointBrokerTopologyBuilder();

            Apply(builder);

            return builder.BuildBrokerTopology();
        }
    }
}
