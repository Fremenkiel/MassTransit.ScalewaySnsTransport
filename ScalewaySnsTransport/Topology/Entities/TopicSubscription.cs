namespace MassTransit.ScalewaySnsTransport.Topology
{
    /// <summary>
    /// The topic to queue binding details to declare the binding to ScalewaySns
    /// </summary>
    public interface TopicSubscription
    {
        /// <summary>
        /// The topic
        /// </summary>
        Topic Source { get; }

        /// <summary>
        /// The queue
        /// </summary>
        Topic Destination { get; }
    }
}
