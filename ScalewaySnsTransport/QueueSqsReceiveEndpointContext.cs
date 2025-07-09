namespace MassTransit.ScalewaySnsTransport
{
    using System;
    using Configuration;
    using Topology;
    using Transports;
    using Util;


    public class QueueSqsReceiveEndpointContext :
        BaseReceiveEndpointContext,
        SqsReceiveEndpointContext
    {
        readonly Recycle<IClientContextSupervisor> _clientContext;
        readonly IScalewaySnsReceiveEndpointConfiguration _configuration;
        readonly IScalewaySnsHostConfiguration _hostConfiguration;

        public QueueSqsReceiveEndpointContext(IScalewaySnsHostConfiguration hostConfiguration, IScalewaySnsReceiveEndpointConfiguration configuration,
            BrokerTopology brokerTopology)
            : base(hostConfiguration, configuration)
        {
            _hostConfiguration = hostConfiguration;
            _configuration = configuration;
            BrokerTopology = brokerTopology;

            _clientContext = new Recycle<IClientContextSupervisor>(() => new ClientContextSupervisor(_hostConfiguration.ConnectionContextSupervisor));
        }

        public BrokerTopology BrokerTopology { get; }

        public IClientContextSupervisor ClientContextSupervisor => _clientContext.Supervisor;

        public override void AddSendAgent(IAgent agent)
        {
            _clientContext.Supervisor.AddSendAgent(agent);
        }

        public override void AddConsumeAgent(IAgent agent)
        {
            _clientContext.Supervisor.AddConsumeAgent(agent);
        }

        public override Exception ConvertException(Exception exception, string message)
        {
            return new ScalewaySnsConnectionException(message + _hostConfiguration.Settings, exception);
        }

        public override void Probe(ProbeContext context)
        {
            context.Add("type", "ScalewaySNS");
            context.Set(new
            {
                _configuration.Settings.EntityName,
                _configuration.Settings.Durable,
                _configuration.Settings.AutoDelete,
                _configuration.Settings.PrefetchCount,
                ConcurrentMessageLimit,
                _configuration.Settings.WaitTimeSeconds,
                _configuration.Settings.PurgeOnStartup
            });

            var topologyScope = context.CreateScope("topology");
            BrokerTopology.Probe(topologyScope);
        }

        protected override ISendTransportProvider CreateSendTransportProvider()
        {
            return new ScalewaySnsSendTransportProvider(_hostConfiguration.ConnectionContextSupervisor, this);
        }

        protected override IPublishTransportProvider CreatePublishTransportProvider()
        {
            return new ScalewaySnsPublishTransportProvider(_hostConfiguration.ConnectionContextSupervisor, this);
        }
    }
}
