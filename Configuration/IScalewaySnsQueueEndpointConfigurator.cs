namespace MassTransit
{
    public interface IScalewaySnsQueueEndpointConfigurator :
        IScalewaySnsQueueConfigurator
    {
        ushort WaitTimeSeconds { set; }

        bool PurgeOnStartup { set; }
    }
}
