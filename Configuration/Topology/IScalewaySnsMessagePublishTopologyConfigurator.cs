namespace MassTransit
{
    public interface IScalewaySnsMessagePublishTopologyConfigurator<TMessage> :
        IMessagePublishTopologyConfigurator<TMessage>,
        IScalewaySnsMessagePublishTopology<TMessage>,
        IScalewaySnsMessagePublishTopologyConfigurator
        where TMessage : class
    {
    }


    public interface IScalewaySnsMessagePublishTopologyConfigurator :
        IMessagePublishTopologyConfigurator,
        IScalewaySnsMessagePublishTopology,
        IScalewaySnsTopicConfigurator
    {
    }
}
