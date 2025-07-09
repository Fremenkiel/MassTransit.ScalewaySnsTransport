namespace MassTransit.ScalewaySnsTransport.Topology
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Configuration;


    public class ScalewaySnsConsumeTopology :
        ConsumeTopology,
        IScalewaySnsConsumeTopologyConfigurator
    {
        readonly IMessageTopology _messageTopology;
        readonly IScalewaySnsPublishTopology _publishTopology;
        readonly IList<IScalewaySnsConsumeTopologySpecification> _specifications;

        public ScalewaySnsConsumeTopology(IMessageTopology messageTopology, IScalewaySnsPublishTopology publishTopology)
            : base(72)
        {
            _messageTopology = messageTopology;
            _publishTopology = publishTopology;

            _specifications = new List<IScalewaySnsConsumeTopologySpecification>();
        }

        IScalewaySnsMessageConsumeTopology<T> IScalewaySnsConsumeTopology.GetMessageTopology<T>()
        {
            return (IScalewaySnsMessageConsumeTopologyConfigurator<T>)base.GetMessageTopology<T>();
        }

        public void AddSpecification(IScalewaySnsConsumeTopologySpecification specification)
        {
            if (specification == null)
                throw new ArgumentNullException(nameof(specification));

            _specifications.Add(specification);
        }

        IScalewaySnsMessageConsumeTopologyConfigurator<T> IScalewaySnsConsumeTopologyConfigurator.GetMessageTopology<T>()
        {
            return (IScalewaySnsMessageConsumeTopologyConfigurator<T>)base.GetMessageTopology<T>();
        }

        public void Apply(IReceiveEndpointBrokerTopologyBuilder builder)
        {
            foreach (var specification in _specifications)
                specification.Apply(builder);

            ForEach<IScalewaySnsMessageConsumeTopologyConfigurator>(x => x.Apply(builder));
        }

        public void Bind(string topicName, Action<IScalewaySnsTopicSubscriptionConfigurator> configure = null)
        {
            var specification = new ConsumerConsumeTopologySpecification(_publishTopology, topicName);

            configure?.Invoke(specification);

            _specifications.Add(specification);
        }

        public override IEnumerable<ValidationResult> Validate()
        {
            return base.Validate().Concat(_specifications.SelectMany(x => x.Validate()));
        }

        protected override IMessageConsumeTopologyConfigurator CreateMessageTopology<T>()
        {
            var messageTopology = new ScalewaySnsMessageConsumeTopology<T>(_messageTopology.GetMessageTopology<T>(), _publishTopology);

            OnMessageTopologyCreated(messageTopology);

            return messageTopology;
        }
    }
}
