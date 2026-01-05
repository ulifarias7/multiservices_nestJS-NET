using RabbitMq.Configurations;
using RabbitMq.Exceptions;
using RabbitMQ.Client;

namespace RabbitMq.Connection
{
    public class RabbitMQConnectionFactory : IRabbitMQConnectionFactory
    {
        private readonly ConnectionSettings _connectionSettings;
        private ConnectionFactory? _connectionFactory;

        public RabbitMQConnectionFactory(
            ConnectionSettings connectionSettings)
        {
            _connectionSettings = connectionSettings ?? throw new ArgumentNullException(nameof(connectionSettings));
            InitialConnection();
        }
        private void InitialConnection()
        {
            _connectionFactory = new ConnectionFactory();

            if (!string.IsNullOrEmpty(_connectionSettings.Url) &&
                _connectionSettings.Url.StartsWith("amqp://"))
            {
                _connectionFactory.Uri = new Uri(_connectionSettings.Url);
            }
            else
            {
                _connectionFactory.HostName = _connectionSettings.HostName;
                _connectionFactory.Port = _connectionSettings.Port;
                _connectionFactory.UserName = _connectionSettings.UserName;
                _connectionFactory.Password = _connectionSettings.Password;
                _connectionFactory.VirtualHost = _connectionSettings.VirtualHost;
            }

            _connectionFactory.AutomaticRecoveryEnabled = true;
            _connectionFactory.NetworkRecoveryInterval = TimeSpan.FromSeconds(5);
            _connectionFactory.RequestedHeartbeat = TimeSpan.FromSeconds(60);
        }

        public async Task<IConnection> CreateConnectionAsync()
        {
            if (_connectionFactory == null)
                throw new RabbitMQConnectionException("conexion a fabrica no inicializada");

            return await _connectionFactory.CreateConnectionAsync();
        }
    }
}
