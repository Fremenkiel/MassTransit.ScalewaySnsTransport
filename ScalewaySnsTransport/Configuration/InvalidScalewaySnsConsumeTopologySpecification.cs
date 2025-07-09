namespace MassTransit.ScalewaySnsTransport.Configuration
{
    using System.Collections.Generic;
    using Topology;


    public class InvalidScalewaySnsConsumeTopologySpecification :
        IScalewaySnsConsumeTopologySpecification
    {
        readonly string _key;
        readonly string _message;

        public InvalidScalewaySnsConsumeTopologySpecification(string key, string message)
        {
            _key = key;
            _message = message;
        }

        public IEnumerable<ValidationResult> Validate()
        {
            yield return this.Failure(_key, _message);
        }

        public void Apply(IReceiveEndpointBrokerTopologyBuilder builder)
        {
        }
    }
}
