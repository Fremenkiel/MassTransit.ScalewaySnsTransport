namespace MassTransit.ScalewaySnsTransport.Topology
{
    using System;
    using System.Collections.Generic;
    using MassTransit.Topology;


    public class ScalewaySnsPublishTopology :
        PublishTopology,
        IScalewaySnsPublishTopologyConfigurator
    {
        readonly IMessageTopology _messageTopology;

        public ScalewaySnsPublishTopology(IMessageTopology messageTopology)
        {
            _messageTopology = messageTopology;

            TopicAttributes = new Dictionary<string, object>();
            TopicSubscriptionAttributes = new Dictionary<string, object>();
            TopicTags = new Dictionary<string, string>();
        }

        public IDictionary<string, object> TopicAttributes { get; private set; }
        public IDictionary<string, object> TopicSubscriptionAttributes { get; private set; }
        public IDictionary<string, string> TopicTags { get; private set; }

        IScalewaySnsMessagePublishTopology<T> IScalewaySnsPublishTopology.GetMessageTopology<T>()
        {
            return GetMessageTopology<T>() as IScalewaySnsMessagePublishTopology<T>;
        }

        IScalewaySnsMessagePublishTopologyConfigurator IScalewaySnsPublishTopologyConfigurator.GetMessageTopology(Type messageType)
        {
            return GetMessageTopology(messageType) as IScalewaySnsMessagePublishTopologyConfigurator;
        }

        public BrokerTopology GetPublishBrokerTopology()
        {
            var builder = new PublishEndpointBrokerTopologyBuilder();

            ForEachMessageType<IScalewaySnsMessagePublishTopology>(x =>
            {
                x.Apply(builder);

                builder.Topic = null;
            });

            return builder.BuildBrokerTopology();
        }

        IScalewaySnsMessagePublishTopologyConfigurator<T> IScalewaySnsPublishTopologyConfigurator.GetMessageTopology<T>()
        {
            return GetMessageTopology<T>() as IScalewaySnsMessagePublishTopologyConfigurator<T>;
        }

        protected override IMessagePublishTopologyConfigurator CreateMessageTopology<T>()
        {
            var messageTopology = new ScalewaySnsMessagePublishTopology<T>(this, _messageTopology.GetMessageTopology<T>());

            OnMessageTopologyCreated(messageTopology);

            return messageTopology;
        }
    }
}
