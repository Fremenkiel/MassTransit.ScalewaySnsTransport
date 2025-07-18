﻿namespace MassTransit.ScalewaySnsTransport
{
    using System;
    using System.Collections.Generic;
    using Amazon.SQS;
    using Amazon.SQS.Model;
    using Context;
    using Transports;


    public sealed class ScalewaySnsReceiveContext :
        BaseReceiveContext,
        ScalewaySnsMessageContext,
        TransportReceiveContext
    {
        readonly ScalewaySnsHeaderProvider _headerProvider;

        public ScalewaySnsReceiveContext(Message message, bool redelivered, SqsReceiveEndpointContext context, ClientContext clientContext,
            ReceiveSettings settings, ConnectionContext connectionContext)
            : base(redelivered, context, settings, clientContext, connectionContext)
        {
            TransportMessage = message;
            TransportMessage.MessageAttributes ??= new Dictionary<string, MessageAttributeValue>();
            TransportMessage.Attributes ??= new Dictionary<string, string>();

            var messageBody = new SqsMessageBody(message);

            Body = messageBody;

            _headerProvider = new ScalewaySnsHeaderProvider(TransportMessage, messageBody);
        }

        protected override IHeaderProvider HeaderProvider => _headerProvider;

        public override MessageBody Body { get; }

        public Message TransportMessage { get; }

        public Dictionary<string, MessageAttributeValue> Attributes => TransportMessage.MessageAttributes;

        public IDictionary<string, object> GetTransportProperties()
        {
            var properties = new Lazy<Dictionary<string, object>>(() => new Dictionary<string, object>());

            if (TransportMessage.Attributes != null)
            {
                if (TransportMessage.Attributes.TryGetValue(MessageSystemAttributeName.MessageGroupId, out var messageGroupId)
                    && !string.IsNullOrWhiteSpace(messageGroupId))
                    properties.Value[ScalewaySnsTransportPropertyNames.GroupId] = messageGroupId;

                if (TransportMessage.Attributes.TryGetValue(MessageSystemAttributeName.MessageDeduplicationId, out var messageDeduplicationId)
                    && !string.IsNullOrWhiteSpace(messageDeduplicationId))
                    properties.Value[ScalewaySnsTransportPropertyNames.DeduplicationId] = messageDeduplicationId;
            }

            return properties.IsValueCreated ? properties.Value : null;
        }
    }
}
