using RabbitMQ.Client;

namespace RabbitMq.Connection
{
    public interface IRabbitMQConnectionFactory
    {
        public Task<IConnection> CreateConnectionAsync();
    }
}
