namespace MassTransit
{
    using System;
    using Amazon;
    using ScalewaySnsTransport;
    using ScalewaySnsTransport.Configuration;
    using Transports;


    /// <summary>
    /// Settings to configure a ScalewaySns host explicitly without requiring the fluent interface
    /// </summary>
    public interface ScalewaySnsHostSettings
    {
        string ServiceURL { get; }
        /// <summary>
        /// The ScalewaySns region to connect
        /// </summary>
        RegionEndpoint Region { get; }

        /// <summary>
        /// The AccessKey for connecting to the host
        /// </summary>
        string SqsAccessKey { get; }

        /// <summary>
        /// The AccessKey for connecting to the host
        /// </summary>
        string SnsAccessKey { get; }

        /// <summary>
        /// The password for connection to the host
        /// MAYBE this should be a SecureString instead of a regular string
        /// </summary>
        string SqsSecretKey { get; }

        /// <summary>
        /// The password for connection to the host
        /// MAYBE this should be a SecureString instead of a regular string
        /// </summary>
        string SnsSecretKey { get; }

        AllowTransportHeader AllowTransportHeader { get; }

        /// <summary>
        /// If true, topics are named "{Scope}_{topicName}" when publishing messages
        /// </summary>
        bool ScopeTopics { get; }

        Uri HostAddress { get; }

        IConnection CreateConnection();
    }
}
