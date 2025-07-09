namespace MassTransit.ScalewaySnsTransport.Configuration
{
    using MassTransit.Configuration;
    using Observables;


    public class ScalewaySnsBusConfiguration :
        ScalewaySnsEndpointConfiguration,
        IScalewaySnsBusConfiguration
    {
        readonly BusObservable _busObservers;

        public ScalewaySnsBusConfiguration(IScalewaySnsTopologyConfiguration topologyConfiguration)
            : base(topologyConfiguration)
        {
            HostConfiguration = new ScalewaySnsHostConfiguration(this, topologyConfiguration);
            BusEndpointConfiguration = CreateEndpointConfiguration(true);

            _busObservers = new BusObservable();
        }

        IHostConfiguration IBusConfiguration.HostConfiguration => HostConfiguration;
        IEndpointConfiguration IBusConfiguration.BusEndpointConfiguration => BusEndpointConfiguration;
        IBusObserver IBusConfiguration.BusObservers => _busObservers;

        public IScalewaySnsEndpointConfiguration BusEndpointConfiguration { get; }
        public IScalewaySnsHostConfiguration HostConfiguration { get; }

        public ConnectHandle ConnectBusObserver(IBusObserver observer)
        {
            return _busObservers.Connect(observer);
        }

        public ConnectHandle ConnectEndpointConfigurationObserver(IEndpointConfigurationObserver observer)
        {
            return HostConfiguration.ConnectEndpointConfigurationObserver(observer);
        }
    }
}
