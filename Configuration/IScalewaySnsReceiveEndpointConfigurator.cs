namespace MassTransit
{
    using System;
    using ScalewaySnsTransport;


    /// <summary>
    /// Configure a receiving ScalewaySns endpoint
    /// </summary>
    public interface IScalewaySnsReceiveEndpointConfigurator :
        IReceiveEndpointConfigurator,
        IScalewaySnsQueueEndpointConfigurator
    {
        /// <summary>
        /// The number of seconds to wait before allowing SQS to redeliver the message when faults are returned back to SQS.
        /// Defaults to 0.
        /// </summary>
        int RedeliverVisibilityTimeout { set; }

        /// <summary>
        /// Set number of concurrent messages per MessageGroupId, higher value will increase throughput but will break delivery order (default: 1).
        /// This applies to FIFO queues only.
        /// </summary>
        int ConcurrentDeliveryLimit { set; }

        public TimeSpan MaxVisibilityTimeout { set; }

        /// <summary>
        /// Bind an existing exchange for the message type to the receive endpoint by name
        /// </summary>
        /// <typeparam name="T"></typeparam>
        void Subscribe<T>(Action<IScalewaySnsTopicSubscriptionConfigurator> callback = null)
            where T : class;

        /// <summary>
        /// Bind an exchange to the receive endpoint exchange
        /// </summary>
        /// <param name="topicName">The exchange name</param>
        /// <param name="callback">Configure the exchange and binding</param>
        void Subscribe(string topicName, Action<IScalewaySnsTopicSubscriptionConfigurator> callback = default);

        void ConfigureClient(Action<IPipeConfigurator<ClientContext>> configure);

        void ConfigureConnection(Action<IPipeConfigurator<ConnectionContext>> configure);

        /// <summary>
        /// FIFO queues deliver messages to consumers partitioned by MessageGroupId, in SequenceNumber order. Calling this method will
        /// disable that behavior.
        /// </summary>
        void DisableMessageOrdering();
    }
}
