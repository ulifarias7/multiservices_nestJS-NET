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
            if (string.IsNullOrEmpty(_connectionSettings.Uri))
                throw new RabbitMQConnectionException("La url es requeridad");

            _connectionFactory = new ConnectionFactory
            {
                Uri = new Uri(_connectionSettings.Uri!),
                AutomaticRecoveryEnabled = true,
                NetworkRecoveryInterval = TimeSpan.FromSeconds(5),
                RequestedHeartbeat = TimeSpan.FromSeconds(60)
            };
        }

        public async Task<IConnection> CreateConnectionAsync()
        {
            if (_connectionFactory == null)
                throw new RabbitMQConnectionException("conexion a fabrica no inicializada");

            return await _connectionFactory.CreateConnectionAsync();
        }
    }
}
