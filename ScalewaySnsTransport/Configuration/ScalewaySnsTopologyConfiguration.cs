namespace MassTransit.ScalewaySnsTransport.Configuration
{
    using System.Collections.Generic;
    using System.Linq;
    using MassTransit.Configuration;
    using Topology;


    public class ScalewaySnsTopologyConfiguration :
        IScalewaySnsTopologyConfiguration
    {
        readonly IScalewaySnsConsumeTopologyConfigurator _consumeTopology;
        readonly IMessageTopologyConfigurator _messageTopology;
        readonly IScalewaySnsPublishTopologyConfigurator _publishTopology;
        readonly IScalewaySnsSendTopologyConfigurator _sendTopology;

        public ScalewaySnsTopologyConfiguration(IMessageTopologyConfigurator messageTopology)
        {
            _messageTopology = messageTopology;

            _sendTopology = new ScalewaySnsSendTopology(ScalewaySnsEntityNameValidator.Validator);
            _sendTopology.ConnectSendTopologyConfigurationObserver(new DelegateSendTopologyConfigurationObserver(GlobalTopology.Send));

            _publishTopology = new ScalewaySnsPublishTopology(messageTopology);
            _publishTopology.ConnectPublishTopologyConfigurationObserver(new DelegatePublishTopologyConfigurationObserver(GlobalTopology.Publish));

            var observer = new PublishToSendTopologyConfigurationObserver(_sendTopology);
            _publishTopology.ConnectPublishTopologyConfigurationObserver(observer);

            _consumeTopology = new ScalewaySnsConsumeTopology(messageTopology, _publishTopology);
        }

        public ScalewaySnsTopologyConfiguration(IScalewaySnsTopologyConfiguration topologyConfiguration)
        {
            _messageTopology = topologyConfiguration.Message;
            _sendTopology = topologyConfiguration.Send;
            _publishTopology = topologyConfiguration.Publish;

            _consumeTopology = new ScalewaySnsConsumeTopology(topologyConfiguration.Message, topologyConfiguration.Publish);
        }

        IMessageTopologyConfigurator ITopologyConfiguration.Message => _messageTopology;
        ISendTopologyConfigurator ITopologyConfiguration.Send => _sendTopology;
        IPublishTopologyConfigurator ITopologyConfiguration.Publish => _publishTopology;
        IConsumeTopologyConfigurator ITopologyConfiguration.Consume => _consumeTopology;

        IScalewaySnsPublishTopologyConfigurator IScalewaySnsTopologyConfiguration.Publish => _publishTopology;
        IScalewaySnsSendTopologyConfigurator IScalewaySnsTopologyConfiguration.Send => _sendTopology;
        IScalewaySnsConsumeTopologyConfigurator IScalewaySnsTopologyConfiguration.Consume => _consumeTopology;

        public IEnumerable<ValidationResult> Validate()
        {
            return _sendTopology.Validate()
                .Concat(_publishTopology.Validate())
                .Concat(_consumeTopology.Validate());
        }
    }
}
