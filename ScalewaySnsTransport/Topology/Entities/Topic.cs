namespace MassTransit.ScalewaySnsTransport.Topology
{
    using System.Collections.Generic;


    /// <summary>
    /// The exchange details used to declare the exchange to ScalewaySns
    /// </summary>
    public interface Topic
    {
        /// <summary>
        /// The exchange name
        /// </summary>
        string EntityName { get; }

        /// <summary>
        /// True if the exchange should be durable, and survive a broker restart
        /// </summary>
        bool Durable { get; }

        /// <summary>
        /// True if the exchange should be deleted when the connection is closed
        /// </summary>
        bool AutoDelete { get; }

        IDictionary<string, object> TopicAttributes { get; }

        IDictionary<string, object> TopicSubscriptionAttributes { get; }

        /// <summary>
        /// Collection of tags to assign to topic when created.
        /// </summary>
        IDictionary<string, string> TopicTags { get; }
    }
}
