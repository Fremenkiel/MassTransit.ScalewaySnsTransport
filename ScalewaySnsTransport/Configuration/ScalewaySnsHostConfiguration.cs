namespace MassTransit.ScalewaySnsTransport.Configuration
{
    using System;
    using MassTransit.Configuration;
    using Topology;
    using Transports;
    using Util;


    public class ScalewaySnsHostConfiguration :
        BaseHostConfiguration<IScalewaySnsReceiveEndpointConfiguration, IScalewaySnsReceiveEndpointConfigurator>,
        IScalewaySnsHostConfiguration
    {
        readonly IScalewaySnsBusConfiguration _busConfiguration;
        readonly IScalewaySnsBusTopology _busTopology;
        readonly Recycle<IConnectionContextSupervisor> _connectionContext;
        readonly IScalewaySnsTopologyConfiguration _topologyConfiguration;
        ScalewaySnsHostSettings _hostSettings;

        public ScalewaySnsHostConfiguration(IScalewaySnsBusConfiguration busConfiguration, IScalewaySnsTopologyConfiguration
            topologyConfiguration)
            : base(busConfiguration)
        {
            _busConfiguration = busConfiguration;
            _topologyConfiguration = topologyConfiguration;

            _hostSettings = new ConfigurationHostSettings();

            var messageNameFormatter = new ScalewaySnsMessageNameFormatter();

            _busTopology = new ScalewaySnsBusTopology(this, messageNameFormatter, topologyConfiguration);

            ReceiveTransportRetryPolicy = Retry.CreatePolicy(x =>
            {
                x.Handle<ScalewaySnsTransportException>();
                x.Handle<ScalewaySnsConnectionException>();

                x.Exponential(1000, TimeSpan.FromSeconds(3), TimeSpan.FromSeconds(30), TimeSpan.FromSeconds(3));
            });

            _connectionContext = new Recycle<IConnectionContextSupervisor>(() => new ConnectionContextSupervisor(this, topologyConfiguration));
        }

        public IConnectionContextSupervisor ConnectionContextSupervisor => _connectionContext.Supervisor;

        public override Uri HostAddress => _hostSettings.HostAddress;

        public ScalewaySnsHostSettings Settings
        {
            get => _hostSettings;
            set
            {
                _hostSettings = value ?? throw new ArgumentNullException(nameof(value));

                var hostAddress = new ScalewaySnsHostAddress(value.HostAddress);

                if (value.ScopeTopics && hostAddress.Scope != "/")
                {
                    var formatter = new PrefixEntityNameFormatter(_topologyConfiguration.Message.EntityNameFormatter, hostAddress.Scope.Trim('/') + "_");

                    _topologyConfiguration.Message.SetEntityNameFormatter(formatter);
                }
            }
        }

        public override IBusTopology Topology => _busTopology;

        public override IRetryPolicy ReceiveTransportRetryPolicy { get; }

        public void ApplyEndpointDefinition(IScalewaySnsReceiveEndpointConfigurator configurator, IEndpointDefinition definition)
        {
            if (definition.IsTemporary)
            {
                configurator.AutoDelete = true;
                configurator.Durable = false;
            }

            base.ApplyEndpointDefinition(configurator, definition);
        }

        public IScalewaySnsReceiveEndpointConfiguration CreateReceiveEndpointConfiguration(string queueName,
            Action<IScalewaySnsReceiveEndpointConfigurator> configure)
        {
            var endpointConfiguration = _busConfiguration.CreateEndpointConfiguration();

            var settings = new QueueReceiveSettings(endpointConfiguration, queueName, true, false);

            return CreateReceiveEndpointConfiguration(settings, endpointConfiguration, configure);
        }

        public IScalewaySnsReceiveEndpointConfiguration CreateReceiveEndpointConfiguration(QueueReceiveSettings settings,
            IScalewaySnsEndpointConfiguration endpointConfiguration, Action<IScalewaySnsReceiveEndpointConfigurator> configure)
        {
            if (settings == null)
                throw new ArgumentNullException(nameof(settings));
            if (endpointConfiguration == null)
                throw new ArgumentNullException(nameof(endpointConfiguration));

            var configuration = new ScalewaySnsReceiveEndpointConfiguration(this, settings, endpointConfiguration);

            configure?.Invoke(configuration);

            Observers.EndpointConfigured(configuration);

            Add(configuration);

            return configuration;
        }

        IScalewaySnsBusTopology IScalewaySnsHostConfiguration.Topology => _busTopology;

        public override void ReceiveEndpoint(IEndpointDefinition definition, IEndpointNameFormatter endpointNameFormatter,
            Action<IScalewaySnsReceiveEndpointConfigurator> configureEndpoint = null)
        {
            var queueName = definition.GetEndpointName(endpointNameFormatter ?? DefaultEndpointNameFormatter.Instance);

            ReceiveEndpoint(queueName, configurator =>
            {
                ApplyEndpointDefinition(configurator, definition);
                configureEndpoint?.Invoke(configurator);
            });
        }

        public override void ReceiveEndpoint(string queueName, Action<IScalewaySnsReceiveEndpointConfigurator> configureEndpoint)
        {
            CreateReceiveEndpointConfiguration(queueName, configureEndpoint);
        }

        public override IReceiveEndpointConfiguration CreateReceiveEndpointConfiguration(string queueName,
            Action<IReceiveEndpointConfigurator> configure = null)
        {
            return CreateReceiveEndpointConfiguration(queueName, configure);
        }

        public override IHost Build()
        {
            var host = new ScalewaySnsHost(this, _busTopology);

            foreach (var endpointConfiguration in GetConfiguredEndpoints())
                endpointConfiguration.Build(host);

            return host;
        }
    }
}
