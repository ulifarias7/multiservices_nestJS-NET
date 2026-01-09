using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace Document.API.Messaging
{
    public class RabbitMqPublisher : IRabbitMqPublisher,
    IHostedService,
    IAsyncDisposable
    {
        private readonly RabbitMqOptions _options;
        private IConnection? _connection;
        private IChannel? _channel;

        public RabbitMqPublisher(RabbitMqOptions options)
        {
            _options = options;
        }

        public async Task StartAsync(CancellationToken ct)
        {
            Console.WriteLine("[RabbitMQ] Iniciando conexión...");

            var factory = new ConnectionFactory
            {
                Uri = new Uri(_options.Uri)
            };

            _connection = await factory.CreateConnectionAsync(ct);
            Console.WriteLine("✅ [RabbitMQ] Conexión creada");

            _channel = await _connection.CreateChannelAsync();
            Console.WriteLine("✅ [RabbitMQ] Canal creado");

            await _channel.QueueDeclareAsync(
                queue: _options.QueueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null,
                cancellationToken: ct
            );
            Console.WriteLine($"La cola fue creada con exito : {_options.QueueName}");
        }

        public async ValueTask PublishAsync<T>(T message, CancellationToken ct = default)
        {
            if (_channel is null)
            {
                Console.WriteLine("❌ [RabbitMQ] Canal no inicializado");
                return;
            }

            var json = JsonSerializer.Serialize(message);
            var body = Encoding.UTF8.GetBytes(json);

            await _channel.BasicPublishAsync(
                exchange: string.Empty,
                routingKey: _options.QueueName,
                body: body,
                cancellationToken: ct
            );

            Console.WriteLine($"📤 [RabbitMQ] Mensaje publicado en '{_options.QueueName}'");
            Console.WriteLine($"📄 Payload: {json}");
        }

        public async Task StopAsync(CancellationToken ct)
        {
            Console.WriteLine("🛑 [RabbitMQ] Cerrando conexión...");

            if (_channel is not null)
                await _channel.CloseAsync(ct);

            if (_connection is not null)
                await _connection.CloseAsync(ct);
        }

        public async ValueTask DisposeAsync()
        {
            if (_channel is not null)
                await _channel.DisposeAsync();

            if (_connection is not null)
                await _connection.DisposeAsync();
        }
    }
}
