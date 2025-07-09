namespace MassTransit.ScalewaySnsTransport.Configuration
{
    using MassTransit.Configuration;
    using Transports;


    public interface IScalewaySnsReceiveEndpointConfiguration :
        IReceiveEndpointConfiguration,
        IScalewaySnsEndpointConfiguration
    {
        ReceiveSettings Settings { get; }

        void Build(IHost host);
    }
}
