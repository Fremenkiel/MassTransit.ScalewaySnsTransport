namespace MassTransit.ScalewaySnsTransport
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Agents;
    using Configuration;
    using Internals;
    using RetryPolicies;
    using Transports;


    public class ConnectionContextFactory :
        IPipeContextFactory<ConnectionContext>
    {
        readonly IScalewaySnsHostConfiguration _hostConfiguration;

        public ConnectionContextFactory(IScalewaySnsHostConfiguration hostConfiguration)
        {
            _hostConfiguration = hostConfiguration;
        }

        public IPipeContextAgent<ConnectionContext> CreateContext(ISupervisor supervisor)
        {
            Task<ConnectionContext> context = Task.Run(() => CreateConnection(supervisor), supervisor.Stopping);

            IPipeContextAgent<ConnectionContext> contextHandle = supervisor.AddContext(context);

            return contextHandle;
        }

        public IActivePipeContextAgent<ConnectionContext> CreateActiveContext(ISupervisor supervisor,
            PipeContextHandle<ConnectionContext> context, CancellationToken cancellationToken)
        {
            return supervisor.AddActiveContext(context, CreateSharedConnection(context.Context, cancellationToken));
        }

        static async Task<ConnectionContext> CreateSharedConnection(Task<ConnectionContext> context, CancellationToken cancellationToken)
        {
            return context.IsCompletedSuccessfully()
                ? new SharedConnectionContext(context.Result, cancellationToken)
                : new SharedConnectionContext(await context.OrCanceled(cancellationToken).ConfigureAwait(false), cancellationToken);
        }

        async Task<ConnectionContext> CreateConnection(ISupervisor supervisor)
        {
            return await _hostConfiguration.ReceiveTransportRetryPolicy.Retry(async () =>
            {
                if (supervisor.Stopping.IsCancellationRequested)
                    throw new ScalewaySnsConnectionException($"The connection is stopping and cannot be used: {_hostConfiguration.HostAddress}");

                IConnection connection = null;
                try
                {
                    TransportLogMessages.ConnectHost(_hostConfiguration.Settings.ToString());

                    connection = _hostConfiguration.Settings.CreateConnection();

                    return new ScalewaySnsConnectionContext(connection, _hostConfiguration, supervisor.Stopped);
                }
                catch (OperationCanceledException)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    LogContext.Warning?.Log(ex, "Connection Failed: {InputAddress}", _hostConfiguration.HostAddress);
                    throw new ScalewaySnsConnectionException("Connect failed: " + _hostConfiguration.Settings, ex);
                }
            }, supervisor.Stopping).ConfigureAwait(false);
        }
    }
}
