namespace MassTransit.ScalewaySnsTransport.Topology
{
    using System;
    using Configuration;
    using Transports;


    public class ScalewaySnsBusTopology :
        BusTopology,
        IScalewaySnsBusTopology
    {
        readonly IScalewaySnsTopologyConfiguration _configuration;
        readonly IScalewaySnsHostConfiguration _hostConfiguration;
        readonly IMessageNameFormatter _messageNameFormatter;

        public ScalewaySnsBusTopology(IScalewaySnsHostConfiguration hostConfiguration, IMessageNameFormatter messageNameFormatter,
            IScalewaySnsTopologyConfiguration configuration)
            : base(hostConfiguration, configuration)
        {
            _hostConfiguration = hostConfiguration;
            _messageNameFormatter = messageNameFormatter;
            _configuration = configuration;
        }

        IScalewaySnsPublishTopology IScalewaySnsBusTopology.PublishTopology => _configuration.Publish;
        IScalewaySnsSendTopology IScalewaySnsBusTopology.SendTopology => _configuration.Send;

        IScalewaySnsMessagePublishTopology<T> IScalewaySnsBusTopology.Publish<T>()
        {
            return _configuration.Publish.GetMessageTopology<T>();
        }

        IScalewaySnsMessageSendTopology<T> IScalewaySnsBusTopology.Send<T>()
        {
            return _configuration.Send.GetMessageTopology<T>();
        }

        public SendSettings GetSendSettings(Uri address)
        {
            var endpointAddress = new ScalewaySnsEndpointAddress(_hostConfiguration.HostAddress, address);

            return _configuration.Send.GetSendSettings(endpointAddress);
        }

        public Uri GetDestinationAddress(string topicName, Action<IScalewaySnsTopicConfigurator> configure = null)
        {
            var address = new ScalewaySnsEndpointAddress(_hostConfiguration.HostAddress, new Uri($"topic:{topicName}"));

            var publishSettings = new TopicPublishSettings(address);

            configure?.Invoke(publishSettings);

            return publishSettings.GetSendAddress(_hostConfiguration.HostAddress);
        }

        public Uri GetDestinationAddress(Type messageType, Action<IScalewaySnsTopicConfigurator> configure = null)
        {
            var topicName = _messageNameFormatter.GetMessageName(messageType).ToString();
            var isTemporary = MessageTypeCache.IsTemporaryMessageType(messageType);
            var address = new ScalewaySnsEndpointAddress(_hostConfiguration.HostAddress, new Uri($"topic:{topicName}?temporary={isTemporary}"));

            var publishSettings = new TopicPublishSettings(address);

            configure?.Invoke(publishSettings);

            return publishSettings.GetSendAddress(_hostConfiguration.HostAddress);
        }
    }
}
