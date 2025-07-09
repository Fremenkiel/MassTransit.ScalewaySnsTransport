namespace MassTransit.ScalewaySnsTransport
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using Context;


    public class ScalewaySnsMessageSendContext<T> :
        MessageSendContext<T>,
        ScalewaySnsSendContext<T>
        where T : class
    {
        public ScalewaySnsMessageSendContext(T message, CancellationToken cancellationToken)
            : base(message, cancellationToken)
        {
        }

        public string GroupId { get; set; }
        public string DeduplicationId { get; set; }

        public int? DelaySeconds
        {
            set => Delay = value.HasValue ? TimeSpan.FromSeconds(value.Value) : default;
        }

        public override void ReadPropertiesFrom(IReadOnlyDictionary<string, object> properties)
        {
            base.ReadPropertiesFrom(properties);

            GroupId = ReadString(properties, ScalewaySnsTransportPropertyNames.GroupId);
            DeduplicationId = ReadString(properties, ScalewaySnsTransportPropertyNames.DeduplicationId);
        }

        public override void WritePropertiesTo(IDictionary<string, object> properties)
        {
            base.WritePropertiesTo(properties);

            if (!string.IsNullOrWhiteSpace(GroupId))
                properties[ScalewaySnsTransportPropertyNames.GroupId] = GroupId;
            if (!string.IsNullOrWhiteSpace(DeduplicationId))
                properties[ScalewaySnsTransportPropertyNames.DeduplicationId] = DeduplicationId;
        }
    }
}
