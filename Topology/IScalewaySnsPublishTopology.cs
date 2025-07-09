namespace MassTransit
{
    using System.Collections.Generic;
    using ScalewaySnsTransport.Topology;


    public interface IScalewaySnsPublishTopology :
        IPublishTopology
    {
        IDictionary<string, object> TopicAttributes { get; }

        IDictionary<string, object> TopicSubscriptionAttributes { get; }

        /// <summary>
        /// Collection of tags to assign to topic when created.
        /// </summary>
        IDictionary<string, string> TopicTags { get; }

        new IScalewaySnsMessagePublishTopology<T> GetMessageTopology<T>()
            where T : class;

        BrokerTopology GetPublishBrokerTopology();
    }
}
