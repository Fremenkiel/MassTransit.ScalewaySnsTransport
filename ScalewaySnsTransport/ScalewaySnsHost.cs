namespace MassTransit.ScalewaySnsTransport
{
    using System;
    using Configuration;
    using Transports;


    public class ScalewaySnsHost :
        BaseHost,
        IScalewaySnsHost
    {
        readonly IScalewaySnsHostConfiguration _hostConfiguration;

        public ScalewaySnsHost(IScalewaySnsHostConfiguration hostConfiguration, IScalewaySnsBusTopology busTopology)
            : base(hostConfiguration, busTopology)
        {
            _hostConfiguration = hostConfiguration;
            Topology = busTopology;
        }

        public new IScalewaySnsBusTopology Topology { get; }

        public override HostReceiveEndpointHandle ConnectReceiveEndpoint(IEndpointDefinition definition, IEndpointNameFormatter endpointNameFormatter,
            Action<IReceiveEndpointConfigurator> configureEndpoint = null)
        {
            return ConnectReceiveEndpoint(definition, endpointNameFormatter, configureEndpoint);
        }

        public override HostReceiveEndpointHandle ConnectReceiveEndpoint(string queueName, Action<IReceiveEndpointConfigurator> configureEndpoint = null)
        {
            return ConnectReceiveEndpoint(queueName, configureEndpoint);
        }

        public HostReceiveEndpointHandle ConnectReceiveEndpoint(IEndpointDefinition definition, IEndpointNameFormatter endpointNameFormatter = null,
            Action<IScalewaySnsReceiveEndpointConfigurator> configureEndpoint = null)
        {
            var queueName = definition.GetEndpointName(endpointNameFormatter ?? DefaultEndpointNameFormatter.Instance);

            return ConnectReceiveEndpoint(queueName, configurator =>
            {
                _hostConfiguration.ApplyEndpointDefinition(configurator, definition);
                configureEndpoint?.Invoke(configurator);
            });
        }

        public HostReceiveEndpointHandle ConnectReceiveEndpoint(string queueName, Action<IScalewaySnsReceiveEndpointConfigurator> configure = null)
        {
            LogContext.SetCurrentIfNull(_hostConfiguration.LogContext);

            var configuration = _hostConfiguration.CreateReceiveEndpointConfiguration(queueName, configure);

            configuration.Validate().ThrowIfContainsFailure("The receive endpoint configuration is invalid:");

            TransportLogMessages.ConnectReceiveEndpoint(configuration.InputAddress);

            configuration.Build(this);

            return ReceiveEndpoints.Start(queueName);
        }

        // TODO: Might need more work here
        protected override void Probe(ProbeContext context)
        {
            context.Set(new
            {
                Type = "ScalewaySNS",
                _hostConfiguration.Settings.Region,
                _hostConfiguration.Settings.SqsAccessKey,
                _hostConfiguration.Settings.SnsAccessKey
            });

            _hostConfiguration.ConnectionContextSupervisor.Probe(context);
        }

        protected override IAgent[] GetAgentHandles()
        {
            return new IAgent[] { _hostConfiguration.ConnectionContextSupervisor };
        }
    }
}
