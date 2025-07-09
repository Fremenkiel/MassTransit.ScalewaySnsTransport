namespace MassTransit.ScalewaySnsTransport
{
    using System;
    using System.Threading.Tasks;
    using Configuration;
    using Middleware;
    using Topology;
    using Transports;


    public class ConnectionContextSupervisor :
        TransportPipeContextSupervisor<ConnectionContext>,
        IConnectionContextSupervisor
    {
        readonly IScalewaySnsHostConfiguration _hostConfiguration;
        readonly IScalewaySnsTopologyConfiguration _topologyConfiguration;

        public ConnectionContextSupervisor(IScalewaySnsHostConfiguration hostConfiguration, IScalewaySnsTopologyConfiguration topologyConfiguration)
            : base(new ConnectionContextFactory(hostConfiguration))
        {
            _hostConfiguration = hostConfiguration;
            _topologyConfiguration = topologyConfiguration;
        }

        public Uri NormalizeAddress(Uri address)
        {
            return new ScalewaySnsEndpointAddress(_hostConfiguration.HostAddress, address);
        }

        public Task<ISendTransport> CreateSendTransport(SqsReceiveEndpointContext receiveEndpointContext, IClientContextSupervisor clientContextSupervisor,
            Uri address)
        {
            LogContext.SetCurrentIfNull(_hostConfiguration.LogContext);

            var endpointAddress = new ScalewaySnsEndpointAddress(_hostConfiguration.HostAddress, address);

            TransportLogMessages.CreateSendTransport(endpointAddress);

            if (endpointAddress.Type == ScalewaySnsEndpointAddress.AddressType.Queue)
            {
                var settings = _topologyConfiguration.Send.GetSendSettings(endpointAddress);

                IPipe<ClientContext> configureTopology = new ConfigureScalewaySnsTopologyFilter<EntitySettings>(settings, settings.GetBrokerTopology()).ToPipe();

                var supervisor = new ClientContextSupervisor(clientContextSupervisor);

                var context = new QueueSendTransportContext(_hostConfiguration, receiveEndpointContext, supervisor, configureTopology, settings.EntityName);

                return CreateTransport(clientContextSupervisor, context);
            }
            else
            {
                var settings = new TopicPublishSettings(endpointAddress);

                var builder = new PublishEndpointBrokerTopologyBuilder();
                var topicHandle = builder.CreateTopic(settings.EntityName, settings.Durable, settings.AutoDelete, settings.TopicAttributes, settings
                    .TopicSubscriptionAttributes, settings.Tags);

                builder.Topic ??= topicHandle;

                IPipe<ClientContext> configureTopology = new ConfigureScalewaySnsTopologyFilter<EntitySettings>(settings, builder.BuildBrokerTopology()).ToPipe();

                var supervisor = new ClientContextSupervisor(clientContextSupervisor);

                var context = new TopicSendTransportContext(_hostConfiguration, receiveEndpointContext, supervisor, configureTopology, settings.EntityName);

                return CreateTransport(clientContextSupervisor, context);
            }
        }

        public Task<ISendTransport> CreatePublishTransport<T>(SqsReceiveEndpointContext receiveEndpointContext,
            IClientContextSupervisor clientContextSupervisor)
            where T : class
        {
            LogContext.SetCurrentIfNull(_hostConfiguration.LogContext);

            IScalewaySnsMessagePublishTopology<T> publishTopology = _topologyConfiguration.Publish.GetMessageTopology<T>();

            var settings = publishTopology.GetPublishSettings(_hostConfiguration.HostAddress);

            IPipe<ClientContext> configureTopology =
                new ConfigureScalewaySnsTopologyFilter<EntitySettings>(settings, publishTopology.GetBrokerTopology()).ToPipe();

            var supervisor = new ClientContextSupervisor(clientContextSupervisor);

            var context = new TopicSendTransportContext(_hostConfiguration, receiveEndpointContext, supervisor, configureTopology, settings.EntityName);

            return CreateTransport(clientContextSupervisor, context);
        }

        static Task<ISendTransport> CreateTransport(IClientContextSupervisor clientContextSupervisor, SendTransportContext<ClientContext> transportContext)
        {
            var transport = new SendTransport<ClientContext>(transportContext);

            clientContextSupervisor.AddSendAgent(transport);

            return Task.FromResult<ISendTransport>(transport);
        }
    }
}
