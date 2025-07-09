namespace MassTransit.ScalewaySnsTransport.Configuration
{
    using System;


    public abstract class EntityConfigurator
    {
        protected EntityConfigurator(string entityName, bool durable = true, bool autoDelete = false)
        {
            EntityName = entityName;
            Durable = durable;
            AutoDelete = autoDelete;
        }

        public bool Durable { get; set; }
        public bool AutoDelete { get; set; }
        public string EntityName { get; set; }

        protected abstract ScalewaySnsEndpointAddress.AddressType AddressType { get; }

        public virtual ScalewaySnsEndpointAddress GetEndpointAddress(Uri hostAddress)
        {
            return new ScalewaySnsEndpointAddress(hostAddress, EntityName, Durable, AutoDelete, AddressType);
        }
    }
}
