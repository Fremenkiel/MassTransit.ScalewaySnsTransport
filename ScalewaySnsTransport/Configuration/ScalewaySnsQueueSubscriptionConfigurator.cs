namespace MassTransit.ScalewaySnsTransport.Configuration
{
    public class ScalewaySnsQueueSubscriptionConfigurator :
        ScalewaySnsQueueConfigurator,
        IScalewaySnsQueueSubscriptionConfigurator
    {
        protected ScalewaySnsQueueSubscriptionConfigurator(string queueName, bool durable, bool autoDelete)
            : base(queueName, durable, autoDelete)
        {
        }
    }
}
