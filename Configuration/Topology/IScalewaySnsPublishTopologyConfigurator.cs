namespace MassTransit
{
    using System;


    public interface IScalewaySnsPublishTopologyConfigurator :
        IPublishTopologyConfigurator,
        IScalewaySnsPublishTopology
    {
        new IScalewaySnsMessagePublishTopologyConfigurator<T> GetMessageTopology<T>()
            where T : class;

        new IScalewaySnsMessagePublishTopologyConfigurator GetMessageTopology(Type messageType);
    }
}
