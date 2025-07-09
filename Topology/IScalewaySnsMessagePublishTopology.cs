namespace MassTransit
{
    using System;
    using ScalewaySnsTransport;
    using ScalewaySnsTransport.Topology;


    public interface IScalewaySnsMessagePublishTopology<TMessage> :
        IMessagePublishTopology<TMessage>,
        IScalewaySnsMessagePublishTopology
        where TMessage : class
    {
        Topic Topic { get; }

        /// <summary>
        /// Returns the send settings for a publish endpoint, which are mostly unused now with topology
        /// </summary>
        /// <returns></returns>
        PublishSettings GetPublishSettings(Uri hostAddress);

        BrokerTopology GetBrokerTopology();
    }


    public interface IScalewaySnsMessagePublishTopology
    {
        /// <summary>
        /// Apply the message topology to the builder, including any implemented types
        /// </summary>
        /// <param name="builder">The topology builder</param>
        void Apply(IPublishEndpointBrokerTopologyBuilder builder);
    }
}
