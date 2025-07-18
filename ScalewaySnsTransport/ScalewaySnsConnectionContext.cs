namespace MassTransit.ScalewaySnsTransport
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Configuration;
    using MassTransit.Middleware;
    using Topology;


    public class ScalewaySnsConnectionContext :
        BasePipeContext,
        ConnectionContext,
        IAsyncDisposable
    {
        readonly IScalewaySnsHostConfiguration _hostConfiguration;

        readonly QueueCache _queueCache;
        readonly TopicCache _topicCache;

        public ScalewaySnsConnectionContext(IConnection connection, IScalewaySnsHostConfiguration hostConfiguration, CancellationToken cancellationToken)
            : base(cancellationToken)
        {
            _hostConfiguration = hostConfiguration;
            Connection = connection;

            Topology = hostConfiguration.Topology;

            _queueCache = new QueueCache(Connection.SqsClient, cancellationToken);
            _topicCache = new TopicCache(Connection.SnsClient, cancellationToken);
        }

        public IConnection Connection { get; }
        public IScalewaySnsBusTopology Topology { get; }

        public Uri HostAddress => _hostConfiguration.HostAddress;

        public Task<QueueInfo> GetQueue(Queue queue)
        {
            return _queueCache.Get(queue);
        }

        public Task<QueueInfo> GetQueueByName(string name)
        {
            return _queueCache.GetByName(name);
        }

        public Task<bool> RemoveQueueByName(string name)
        {
            return _queueCache.RemoveByName(name);
        }

        public Task<TopicInfo> GetTopic(Topic topic)
        {
            return _topicCache.Get(topic);
        }

        public Task<TopicInfo> GetTopicByName(string name)
        {
            return _topicCache.GetByName(name);
        }

        public Task<bool> RemoveTopicByName(string name)
        {
            return _topicCache.RemoveByName(name);
        }

        public ClientContext CreateClientContext(CancellationToken cancellationToken)
        {
            return new ScalewaySnsClientContext(this, Connection.SqsClient, Connection.SnsClient, cancellationToken);
        }

        public async ValueTask DisposeAsync()
        {
            await _queueCache.DisposeAsync().ConfigureAwait(false);

            await _topicCache.DisposeAsync().ConfigureAwait(false);

            Connection?.Dispose();
        }
    }
}
