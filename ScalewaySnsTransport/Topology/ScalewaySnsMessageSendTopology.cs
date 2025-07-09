namespace MassTransit.ScalewaySnsTransport.Topology
{
    using MassTransit.Topology;


    public class ScalewaySnsMessageSendTopology<TMessage> :
        MessageSendTopology<TMessage>,
        IScalewaySnsMessageSendTopologyConfigurator<TMessage>
        where TMessage : class
    {
    }
}
