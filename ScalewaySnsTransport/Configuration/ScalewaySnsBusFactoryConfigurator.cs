#nullable enable
namespace MassTransit.ScalewaySnsTransport.Configuration
{
    using System;
    using System.Collections.Generic;
    using MassTransit.Configuration;
    using Topology;


    public class ScalewaySnsBusFactoryConfigurator :
        BusFactoryConfigurator,
        IScalewaySnsBusFactoryConfigurator,
        IBusFactory
    {
        readonly IScalewaySnsBusConfiguration _busConfiguration;
        readonly IScalewaySnsHostConfiguration _hostConfiguration;
        readonly QueueReceiveSettings _settings;

        public ScalewaySnsBusFactoryConfigurator(IScalewaySnsBusConfiguration busConfiguration)
            : base(busConfiguration)
        {
            _busConfiguration = busConfiguration;
            _hostConfiguration = busConfiguration.HostConfiguration;

            // TODO: Redo this
            var queueName = _busConfiguration.Topology.Consume.CreateTemporaryQueueName("bus");
            _settings = new QueueReceiveSettings(busConfiguration.BusEndpointConfiguration, queueName, false, true);
        }

        public ushort WaitTimeSeconds
        {
            set => _settings.WaitTimeSeconds = value;
        }

        public bool Durable
        {
            set => _settings.Durable = value;
        }

        public bool AutoDelete
        {
            set => _settings.AutoDelete = value;
        }

        public bool PurgeOnStartup
        {
            set => _settings.PurgeOnStartup = value;
        }

        public void OverrideDefaultBusEndpointQueueName(string value)
        {
            _settings.EntityName = value;
        }

        public string Region { get; set; }

        public IDictionary<string, object> QueueAttributes => _settings.QueueAttributes;
        public IDictionary<string, object> QueueSubscriptionAttributes => _settings.QueueSubscriptionAttributes;
        public IDictionary<string, string> QueueTags => _settings.QueueTags;

        public void Host(ScalewaySnsHostSettings settings)
        {
            _busConfiguration.HostConfiguration.Settings = settings;
        }

        void IScalewaySnsBusFactoryConfigurator.Send<T>(Action<IScalewaySnsMessageSendTopologyConfigurator<T>> configureTopology)
        {
            IScalewaySnsMessageSendTopologyConfigurator<T> configurator = _busConfiguration.Topology.Send.GetMessageTopology<T>();

            configureTopology?.Invoke(configurator);
        }

        void IScalewaySnsBusFactoryConfigurator.Publish<T>(Action<IScalewaySnsMessagePublishTopologyConfigurator<T>>? configureTopology)
        {
            IScalewaySnsMessagePublishTopologyConfigurator<T> configurator = _busConfiguration.Topology.Publish.GetMessageTopology<T>();

            configureTopology?.Invoke(configurator);
        }

        public void Publish(Type messageType, Action<IScalewaySnsMessagePublishTopologyConfigurator>? configure = null)
        {
            var configurator = _busConfiguration.Topology.Publish.GetMessageTopology(messageType);

            configure?.Invoke(configurator);
        }

        public new IScalewaySnsSendTopologyConfigurator SendTopology => _busConfiguration.Topology.Send;
        public new IScalewaySnsPublishTopologyConfigurator PublishTopology => _busConfiguration.Topology.Publish;

        public void ReceiveEndpoint(IEndpointDefinition definition, IEndpointNameFormatter? endpointNameFormatter,
            Action<IScalewaySnsReceiveEndpointConfigurator>? configureEndpoint)
        {
            _hostConfiguration.ReceiveEndpoint(definition, endpointNameFormatter, configureEndpoint);
        }

        public void ReceiveEndpoint(IEndpointDefinition definition, IEndpointNameFormatter? endpointNameFormatter,
            Action<IReceiveEndpointConfigurator>? configureEndpoint)
        {
            _hostConfiguration.ReceiveEndpoint(definition, endpointNameFormatter, configureEndpoint);
        }

        public void ReceiveEndpoint(string queueName, Action<IScalewaySnsReceiveEndpointConfigurator> configureEndpoint)
        {
            _hostConfiguration.ReceiveEndpoint(queueName, configureEndpoint);
        }

        public void ReceiveEndpoint(string queueName, Action<IReceiveEndpointConfigurator> configureEndpoint)
        {
            _hostConfiguration.ReceiveEndpoint(queueName, configureEndpoint);
        }

        public IReceiveEndpointConfiguration CreateBusEndpointConfiguration(Action<IReceiveEndpointConfigurator> configure)
        {
            return _busConfiguration.HostConfiguration.CreateReceiveEndpointConfiguration(_settings, _busConfiguration.BusEndpointConfiguration, configure);
        }

        public override IEnumerable<ValidationResult> Validate()
        {
            foreach (var result in base.Validate())
                yield return result;

            if (string.IsNullOrWhiteSpace(_settings.EntityName))
                yield return this.Failure("Bus", "The bus queue name must not be null or empty");
        }
    }
}
