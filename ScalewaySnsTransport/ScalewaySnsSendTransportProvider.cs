namespace MassTransit.ScalewaySnsTransport
{
    using System;
    using System.Threading.Tasks;
    using Transports;


    public class ScalewaySnsSendTransportProvider :
        ISendTransportProvider
    {
        readonly IClientContextSupervisor _clientContextSupervisor;
        readonly IConnectionContextSupervisor _connectionContextSupervisor;
        readonly SqsReceiveEndpointContext _context;

        public ScalewaySnsSendTransportProvider(IConnectionContextSupervisor connectionContextSupervisor, SqsReceiveEndpointContext context)
        {
            _connectionContextSupervisor = connectionContextSupervisor;
            _context = context;
            _clientContextSupervisor = context.ClientContextSupervisor;
        }

        public Uri NormalizeAddress(Uri address)
        {
            return _connectionContextSupervisor.NormalizeAddress(address);
        }

        public Task<ISendTransport> GetSendTransport(Uri address)
        {
            return _connectionContextSupervisor.CreateSendTransport(_context, _clientContextSupervisor, address);
        }
    }
}
