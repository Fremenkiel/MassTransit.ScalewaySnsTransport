namespace MassTransit.ScalewaySnsTransport.Topology
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Configuration;


    public class ScalewaySnsMessageConsumeTopology<TMessage> :
        MessageConsumeTopology<TMessage>,
        IScalewaySnsMessageConsumeTopologyConfigurator<TMessage>,
        IScalewaySnsMessageConsumeTopologyConfigurator
        where TMessage : class
    {
        readonly IScalewaySnsMessagePublishTopology<TMessage> _messagePublishTopology;
        readonly IMessageTopology<TMessage> _messageTopology;
        readonly IScalewaySnsPublishTopology _publishTopology;
        readonly IList<IScalewaySnsConsumeTopologySpecification> _specifications;

        public ScalewaySnsMessageConsumeTopology(IMessageTopology<TMessage> messageTopology, IScalewaySnsPublishTopology publishTopology)
        {
            _messageTopology = messageTopology;
            _publishTopology = publishTopology;
            _messagePublishTopology = _publishTopology.GetMessageTopology<TMessage>();

            _specifications = new List<IScalewaySnsConsumeTopologySpecification>();
        }

        public void Apply(IReceiveEndpointBrokerTopologyBuilder builder)
        {
            foreach (var specification in _specifications)
                specification.Apply(builder);
        }

        public void Subscribe(Action<IScalewaySnsTopicSubscriptionConfigurator> configure = null)
        {
            if (!IsBindableMessageType)
            {
                _specifications.Add(new InvalidScalewaySnsConsumeTopologySpecification(TypeCache<TMessage>.ShortName, "Is not a bindable message type"));
                return;
            }

            var specification = new ConsumerConsumeTopologySpecification(_publishTopology, _messagePublishTopology.Topic);

            configure?.Invoke(specification);

            _specifications.Add(specification);
        }

        public override IEnumerable<ValidationResult> Validate()
        {
            return base.Validate().Concat(_specifications.SelectMany(x => x.Validate()));
        }
    }
}
