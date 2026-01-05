using RabbitMq.Connection;
using RabbitMq.Exceptions;
using RabbitMQ.Client;

namespace RabbitMq.Channel
{
    public class ChannelManager : IChannelManager, IDisposable
    {
        private readonly IRabbitMQConnectionFactory _rabbitMQConnection;
        private IConnection? _connection;
        private readonly SemaphoreSlim _semaphore = new(1, 1);
        private bool _disposed;

        public ChannelManager(IRabbitMQConnectionFactory rabbitMQConnection)
        {
            _rabbitMQConnection = rabbitMQConnection;
        }

        public async Task CloseChannelAsync(IChannel channel)
        {
            if (channel == null) return;

            if (channel.IsOpen)
            {
                await channel.CloseAsync();
            }

            channel.Dispose();
        }

        public async Task<IChannel> CreateChannelAsync()    
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(ChannelManager));

            await _semaphore.WaitAsync();
            try
            {
                if (_connection == null || !_connection.IsOpen)
                {
                    _connection?.Dispose();
                    _connection = await _rabbitMQConnection.CreateConnectionAsync();
                }

                var channel = await _connection.CreateChannelAsync();

                await channel.BasicQosAsync(
                    prefetchSize: 0,
                    prefetchCount: 10,
                    global: false
                );

                return channel;
            }
            catch (Exception ex)
            {
                throw new RabbitMQChannelException("Erro al crear el canal", ex);
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public void Dispose()
        {
            if (_disposed) return;

            _connection?.Dispose();
            _semaphore?.Dispose();
            _disposed = true;

            GC.SuppressFinalize(this);
        }
    }
}
