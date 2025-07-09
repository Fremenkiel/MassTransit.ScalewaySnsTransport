namespace MassTransit
{
    public interface IScalewaySnsMessageConsumeTopology<TMessage> :
        IMessageConsumeTopology<TMessage>
        where TMessage : class
    {
    }
}
