namespace MassTransit
{
    public interface IScalewaySnsMessageSendTopologyConfigurator<TMessage> :
        IMessageSendTopologyConfigurator<TMessage>,
        IScalewaySnsMessageSendTopology<TMessage>,
        IScalewaySnsMessageSendTopologyConfigurator
        where TMessage : class
    {
    }


    public interface IScalewaySnsMessageSendTopologyConfigurator :
        IMessageSendTopologyConfigurator
    {
    }
}
