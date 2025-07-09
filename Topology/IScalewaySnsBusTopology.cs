namespace MassTransit
{
    using System;
    using ScalewaySnsTransport;


    public interface IScalewaySnsBusTopology :
        IBusTopology
    {
        new IScalewaySnsPublishTopology PublishTopology { get; }

        new IScalewaySnsSendTopology SendTopology { get; }

        /// <summary>
        /// Returns the destination address for the specified topic
        /// </summary>
        /// <param name="topicName"></param>
        /// <param name="configure">Callback to configure exchange settings</param>
        /// <returns></returns>
        Uri GetDestinationAddress(string topicName, Action<IScalewaySnsTopicConfigurator> configure = null);

        /// <summary>
        /// Returns the destination address for the topic identified by the message type
        /// </summary>
        /// <param name="messageType">The message type</param>
        /// <param name="configure">Callback to configure exchange settings</param>
        /// <returns></returns>
        Uri GetDestinationAddress(Type messageType, Action<IScalewaySnsTopicConfigurator> configure = null);

        /// <summary>
        /// Returns the settings for sending to the specified address. Will parse any arguments
        /// off the query string to properly configure the settings, including exchange and queue
        /// durability, etc.
        /// </summary>
        /// <param name="address">The ScalewaySns endpoint address</param>
        /// <returns>The send settings for the address</returns>
        SendSettings GetSendSettings(Uri address);

        new IScalewaySnsMessagePublishTopology<T> Publish<T>()
            where T : class;

        new IScalewaySnsMessageSendTopology<T> Send<T>()
            where T : class;
    }
}
