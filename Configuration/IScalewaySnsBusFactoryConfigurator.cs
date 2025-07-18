﻿#nullable enable
namespace MassTransit
{
    using System;


    public interface IScalewaySnsBusFactoryConfigurator :
        IBusFactoryConfigurator<IScalewaySnsReceiveEndpointConfigurator>,
        IScalewaySnsQueueEndpointConfigurator
    {
        new IScalewaySnsSendTopologyConfigurator SendTopology { get; }

        new IScalewaySnsPublishTopologyConfigurator PublishTopology { get; }

        /// <summary>
        /// Configure the send topology of the message type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="configureTopology"></param>
        void Send<T>(Action<IScalewaySnsMessageSendTopologyConfigurator<T>> configureTopology)
            where T : class;

        /// <summary>
        /// Configure the send topology of the message type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="configureTopology"></param>
        void Publish<T>(Action<IScalewaySnsMessagePublishTopologyConfigurator<T>>? configureTopology = null)
            where T : class;

        void Publish(Type messageType, Action<IScalewaySnsMessagePublishTopologyConfigurator>? configure = null);

        /// <summary>
        /// In most cases, this is not needed and should not be used. However, if for any reason the default bus
        /// endpoint queue name needs to be changed, this will do it. Do NOT set it to the same name as a receive
        /// endpoint or you will screw things up.
        /// </summary>
        void OverrideDefaultBusEndpointQueueName(string value);

        /// <summary>
        /// Configure a Host that can be connected. If only one host is specified, it is used as the default
        /// host for receive endpoints.
        /// </summary>
        /// <param name="settings"></param>
        /// <returns></returns>
        void Host(ScalewaySnsHostSettings settings);

        string Region { get; set; }
    }
}
