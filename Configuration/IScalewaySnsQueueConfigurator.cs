namespace MassTransit
{
    using System.Collections.Generic;


    /// <summary>
    /// Configures a queue/exchange pair in ScalewaySns
    /// </summary>
    public interface IScalewaySnsQueueConfigurator
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

        IDictionary<string, object> QueueAttributes { get; }

        IDictionary<string, object> QueueSubscriptionAttributes { get; }

        /// <summary>
        /// Collection of tags to assign to queue when created.
        /// </summary>
        IDictionary<string, string> QueueTags { get; }
    }
}
