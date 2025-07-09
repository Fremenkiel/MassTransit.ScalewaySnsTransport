namespace MassTransit.ScalewaySnsTransport
{
    using System;
    using System.Collections.Generic;


    /// <summary>
    /// Specify the receive settings for a receive transport
    /// </summary>
    public interface ReceiveSettings :
        EntitySettings
    {
        /// <summary>
        /// The number of unacknowledged messages to allow to be processed concurrently
        /// </summary>
        int PrefetchCount { get; }

        int ConcurrentMessageLimit { get; }

        int ConcurrentDeliveryLimit { get; }

        int WaitTimeSeconds { get; }

        /// <summary>
        /// If True, and a queue name is specified, if the queue exists and has messages, they are purged at startup
        /// If the connection is reset, messages are not purged until the service is reset
        /// </summary>
        bool PurgeOnStartup { get; }

        IDictionary<string, object> QueueAttributes { get; }

        IDictionary<string, object> QueueSubscriptionAttributes { get; }

        /// <summary>
        /// If the queue is ordered, enables grouping by MessageGroupId and process messages in ordered way by SequenceNumber
        /// </summary>
        bool IsOrdered { get; }

        int VisibilityTimeout { get; set; }

        TimeSpan MaxVisibilityTimeout { get; set; }

        /// <summary>
        /// The number of seconds to wait before allowing SQS to redeliver the message when faults are returned back to SQS.
        /// </summary>
        int RedeliverVisibilityTimeout { get; set; }

        string QueueUrl { get; set; }

        /// <summary>
        /// Get the input address for the transport on the specified host
        /// </summary>
        Uri GetInputAddress(Uri hostAddress);
    }
}
