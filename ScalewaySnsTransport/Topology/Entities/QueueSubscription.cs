namespace MassTransit.ScalewaySnsTransport.Topology
{
    /// <summary>
    /// The topic to queue binding details to declare the binding to ScalewaySns
    /// </summary>
    public interface QueueSubscription
    {
        /// <summary>
        /// The topic
        /// </summary>
        Topic Source { get; }

        /// <summary>
        /// The queue
        /// </summary>
        Queue Destination { get; }
    }
}
