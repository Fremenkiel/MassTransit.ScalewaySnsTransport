namespace MassTransit
{
    using System;
    using Configuration;


    public static class ScalewaySnsMessageSchedulerExtensions
    {
        /// <summary>
        /// Uses the Amazon SQS delayed messages to schedule messages for future delivery. A lightweight
        /// alternative to Quartz, which does not require any storage outside of ScalewaySns.
        /// </summary>
        /// <param name="configurator"></param>
        [Obsolete("Use the transport independent UseDelayedMessageScheduler")]
        public static void UseScalewaySnsMessageScheduler(this IBusFactoryConfigurator configurator)
        {
            if (configurator == null)
                throw new ArgumentNullException(nameof(configurator));

            var specification = new DelayedMessageSchedulerSpecification();

            configurator.AddPipeSpecification(specification);
        }

        /// <summary>
        /// Add a <see cref="IMessageScheduler" /> to the container that uses the SQS message delay to schedule messages.
        /// </summary>
        /// <param name="configurator"></param>
        [Obsolete("Use the transport independent AddDelayedMessageScheduler")]
        public static void AddScalewaySnsMessageScheduler(this IBusRegistrationConfigurator configurator)
        {
            configurator.AddDelayedMessageScheduler();
        }
    }
}
