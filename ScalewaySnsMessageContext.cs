namespace MassTransit
{
    using System.Collections.Generic;
    using Amazon.SQS.Model;


    public interface ScalewaySnsMessageContext
    {
        Message TransportMessage { get; }

        Dictionary<string, MessageAttributeValue> Attributes { get; }
    }
}
