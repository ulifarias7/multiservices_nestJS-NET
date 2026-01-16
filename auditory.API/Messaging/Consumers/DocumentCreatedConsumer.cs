using Auditory.API.Database.Entity;
using Auditory.API.Database.Persistence;
using Auditory.API.Messaging.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace Auditory.API.Messaging.Consumers
{
    public class DocumentCreatedConsumer : BackgroundService
    {
        private readonly RabbitMqDocumentServiceOptions _rabbitMqOptions;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<DocumentCreatedConsumer> _logger;
        private IConnection? _connection;
        private IChannel? _channel;

        public DocumentCreatedConsumer(
            RabbitMqDocumentServiceOptions rabbitMqOptions,
            IServiceScopeFactory scopeFactory,
            ILogger<DocumentCreatedConsumer> logger)
        {
            _rabbitMqOptions = rabbitMqOptions;
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var factory = new ConnectionFactory
            {
                Uri = new Uri(_rabbitMqOptions.Uri)
            };

            _connection = await factory.CreateConnectionAsync(stoppingToken);
            _logger.LogInformation($"esta es la ruta de conexion{factory.Uri}");
            _channel = await _connection.CreateChannelAsync();
            _logger.LogInformation($"se genero la conexion");

            await _channel.QueueDeclareAsync(
                queue: _rabbitMqOptions.QueueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null,
                cancellationToken: stoppingToken
            );

            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.ReceivedAsync += OnMessageReceived;

            await _channel.BasicConsumeAsync(
                queue: _rabbitMqOptions.QueueName,
                autoAck: false,
                consumer: consumer,
                cancellationToken: stoppingToken
            );
        }

        private async Task OnMessageReceived(object sender, BasicDeliverEventArgs args)
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            try
            {
                var body = Encoding.UTF8.GetString(args.Body.ToArray());
                var payloadJson = JsonDocument.Parse(body);

                var audit = new AuditEntity
                {
                    EventName = "DocumentCreated",
                    RoutingKey = args.RoutingKey,
                    Exchange = args.Exchange,
                    SourceService = _rabbitMqOptions.SourceService,
                    SourceHost = Environment.MachineName,
                    Payload = payloadJson,
                    Headers = args.BasicProperties.Headers is not null
                        ? JsonDocument.Parse(JsonSerializer.Serialize(args.BasicProperties.Headers))
                        : null,
                    CorrelationId = args.BasicProperties.CorrelationId,
                    CausationId = null,
                    MessageId = args.BasicProperties.MessageId ?? Guid.NewGuid().ToString(),
                    OccurredAt = ExtractOccurredAt(payloadJson),
                    ReceivedAt = DateTime.UtcNow
                };

                db.Audits.Add(audit);
                await db.SaveChangesAsync();

                await _channel!.BasicAckAsync(args.DeliveryTag, false);
            }
            catch
            {
                await _channel!.BasicNackAsync(args.DeliveryTag, false, true);
            }
        }

        private static DateTime ExtractOccurredAt(JsonDocument payload)
        {
            if (payload.RootElement.TryGetProperty("CreatedAt", out var createdAt))
                return createdAt.GetDateTime();

            return DateTime.UtcNow;
        }

        public override async Task StopAsync(CancellationToken cancellationToken)//se ejeuta cuando la app se esta apagando 
        {
            if (_channel != null) await _channel.CloseAsync();
            if (_connection != null) await _connection.CloseAsync();
        }
    }
}
