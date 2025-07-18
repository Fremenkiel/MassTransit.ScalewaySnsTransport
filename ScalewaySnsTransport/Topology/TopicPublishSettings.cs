namespace MassTransit.ScalewaySnsTransport.Topology
{
    using System;
    using System.Collections.Generic;
    using Configuration;


    public class TopicPublishSettings :
        ScalewaySnsTopicConfigurator,
        PublishSettings
    {
        public TopicPublishSettings(ScalewaySnsEndpointAddress address)
            : base(address.Name, address.Durable, address.AutoDelete)
        {
        }

        public Uri GetSendAddress(Uri hostAddress)
        {
            return GetEndpointAddress(hostAddress);
        }

        IEnumerable<string> GetSettingStrings()
        {
            if (Durable)
                yield return "durable";

            if (AutoDelete)
                yield return "auto-delete";
        }

        public override string ToString()
        {
            return string.Join(", ", GetSettingStrings());
        }
    }
}
