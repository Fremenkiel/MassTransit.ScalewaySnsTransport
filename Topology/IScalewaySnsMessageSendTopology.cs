namespace MassTransit
{
    public interface IScalewaySnsMessageSendTopology<TMessage> :
        IMessageSendTopology<TMessage>
        where TMessage : class
    {
    }
}
