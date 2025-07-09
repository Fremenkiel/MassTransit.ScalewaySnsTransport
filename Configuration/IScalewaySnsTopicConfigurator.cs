namespace MassTransit
{
    using System;
    using System.Collections.Generic;


    /// <summary>
    /// Configures an exchange for ScalewaySns
    /// </summary>
    public interface IScalewaySnsTopicConfigurator
    {
        /// <summary>
        /// Specify the queue should be durable (survives broker restart) or in-memory
        /// </summary>
        /// <value>True for a durable queue, False for an in-memory queue</value>
        bool Durable { set; }

        /// <summary>
        /// Specify that the queue (and the exchange of the same name) should be created as auto-delete
        /// </summary>
        bool AutoDelete { set; }

        IDictionary<string, object> TopicAttributes { get; }

        IDictionary<string, object> TopicSubscriptionAttributes { get; }

        /// <summary>
        /// Collection of tags to assign to topic when created.
        /// </summary>
        IDictionary<string, string> TopicTags { get; }

        ScalewaySnsEndpointAddress GetEndpointAddress(Uri hostAddress);
    }
}
